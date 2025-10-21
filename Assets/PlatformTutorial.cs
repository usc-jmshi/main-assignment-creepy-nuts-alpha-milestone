using UnityEngine;

public class PlatformTutorial : MonoBehaviour
{
    public int Index { get; set; }

    [SerializeField]
    private MeshRenderer _mr;

    private LightType _lightType;
    public bool manualLight;

    public void SetLightType(LightType lightType)
    {
        _lightType = lightType;

        OnLightSet();
    }

    private void OnLightSet()
    {
        _mr.enabled = _lightType == LightManager.Instance.LightType;
    }

    private void Awake()
    {
        LightManager.Instance.LightTypeSet += OnLightSet;

        if (manualLight == true)
        {
            SetLightType(LightType.Blue);
        }
    }
}