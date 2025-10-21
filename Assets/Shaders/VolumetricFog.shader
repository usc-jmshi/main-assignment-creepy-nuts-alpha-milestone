Shader "CreepyNuts/VolumetricFog"
{
    Properties
    {
        // Volumetric Fog Parameters
        _FogColor("Fog Color", Color) = (1,1,1,1)
        _StepSize("Step Size", float) = 0.1
        _MaxStepDist("Max Step Distance", float) = 100
        _DensityMultiplier("Global Density multiplier", Range(0, 10)) = 1
        _FogNoise("Fog Noise Map", 3D) = "white" {}

        // Scatter Light Parameters
        [HDR]_LightContribution("Light contribution", Color) = (1, 1, 1, 1)
        _LightScattering("Light scattering", Range(0, 1)) = 0.2

        // Linear Fog Parameters
        _Start("Start distance", float) = 0
        _End("End distance", float) = 100
        _FallOffScale("Fall Off Scale", Range(0, 10)) = 1

    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            float4 _Color;
            float _StepSize;
            float _MaxStepDist;
            float _DensityMultiplier;
            TEXTURE3D(_FogNoise);
            float4 _LightContribution;
            float _LightScattering;
            float _Start;
            float _End;
            float _FallOffScale;
            
            float rand_1_05(float2 uv)
            {
                float2 noise = (frac(sin(dot(uv ,float2(12.9898,78.233)*2.0)) * 43758.5453));
                return frac(noise.x + noise.y);
            }

            float calculate_volume_density(float3 worldPos)
            {
                float4 noise = _FogNoise.SampleLevel(sampler_TrilinearRepeat, worldPos * 0.1 + float3(_Time.y * 0.1 * sin(_Time.y * 0.001), _Time.y * 0.02, _Time.y * 0.02), 0);
                float density = dot(noise, noise);
                density = saturate(density - 0.2) * _DensityMultiplier;
                return density;
            }

            float calculate_Llinear_density(float3 worldPos)
            {
                float3 viewDir = worldPos - _WorldSpaceCameraPos;
                float viewLength = length(viewDir);
                return exp(-_FallOffScale * viewLength/(_End - _Start));
            }

            half4 frag(Varyings i) : SV_Target
            {
                float4 col = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, i.texcoord);
                float depth = SampleSceneDepth(i.texcoord);
                float3 worldPos = ComputeWorldSpacePosition(i.texcoord, depth, UNITY_MATRIX_I_VP);

                float3 entryPoint = _WorldSpaceCameraPos;
                float3 viewDir = worldPos - _WorldSpaceCameraPos;
                float viewLength = length(viewDir);
                float3 rayDir = normalize(viewDir);

                float2 pixelCoords = i.texcoord * _BlitTexture_TexelSize.zw;
                float distLimit = min(viewLength, _MaxStepDist);
                float distTravelled = InterleavedGradientNoise(pixelCoords, (int)(_Time.y / max(HALF_EPS, unity_DeltaTime.x)));
                float transmittance = 1;
                float4 fogCol = _Color;

                while(distTravelled < distLimit)
                {
                    float3 rayPos = entryPoint + rayDir * distTravelled;
                    float density = calculate_volume_density(rayPos);
                    if (density > 0)
                    {
                        Light mainLight = GetMainLight(TransformWorldToShadowCoord(rayPos));
                        fogCol.rgb *= mainLight.color.rgb * _LightContribution.rgb * pow(saturate(dot(rayDir, mainLight.direction)), _LightScattering) * density * mainLight.shadowAttenuation * _StepSize;
                        transmittance *= exp(-density * _StepSize);
                    }
                    distTravelled += _StepSize;
                }
                
                //return lerp(col, fogCol,  1.0 - saturate(transmittance));
                if(viewLength > _End){
                    return lerp(col, _Color,  1.0 - calculate_Llinear_density(worldPos));
                }
                //return lerp(lerp(col, _Color,  1.0 - calculate_Llinear_density(worldPos)), lerp(col, fogCol,  1.0 - saturate(transmittance)), calculate_Llinear_density(worldPos));
                return lerp(col, _Color,  1.0 - calculate_Llinear_density(worldPos)) * lerp(col, fogCol,  1.0 - saturate(transmittance));
            }
            ENDHLSL
        }
    }
}
