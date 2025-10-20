using UnityEngine;

public class FogEffector : CollectibleEffector
{
    public float depleteSpeed;
    public Material fogMat;
    public float maxVisualDist = 100;
    public float minVisualDist = 10;
    public float maxFogDensity = 0.4f;
    public float minFogDensity = 0.1f;
    public override void OnEffect()
    {
        CollectibleManager.Instance.health -= depleteSpeed * Time.deltaTime;
        if(CollectibleManager.Instance.health < 0)
        {
            CollectibleManager.Instance.health = 0;
        }

        UpdateFog();
    }

    private void UpdateFog()
    {
        float percent = CollectibleManager.Instance.health / CollectibleManager.Instance.maxHP;
        fogMat.SetFloat("_DensityMultiplier", Mathf.Lerp(maxFogDensity, minFogDensity, percent));
        fogMat.SetFloat("_End", Mathf.Lerp(minVisualDist, maxVisualDist, percent));
    }
}
