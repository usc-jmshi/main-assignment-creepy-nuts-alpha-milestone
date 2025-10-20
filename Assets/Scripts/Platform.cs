using UnityEngine;

public class Platform: MonoBehaviour {
  public int Index { get; set; }

  [SerializeField]
  private MeshRenderer _mr;

  private LightType _lightType;
  public bool manualLight;

  public void SetLightType(LightType lightType) {
    _lightType = lightType;

    OnLightSet();
  }

  private void OnLightSet() {
    _mr.enabled = _lightType == LightManager.Instance.LightType;
  }

  private void Awake() {
    LightManager.Instance.LightTypeSet += OnLightSet;

    if (manualLight == true)
        {
      SetLightType(LightType.Blue);
        }
  }

  private void OnDestroy() {
    LightManager.Instance.LightTypeSet -= OnLightSet;
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (!collision.gameObject.TryGetComponent(out PlayerController _))
    {
      return;
    }


    if (Vector3.Dot(collision.GetContact(0).normal, -transform.up) > 0)
    {
      PlatformManager.Instance.CreatePlatforms(Index);
    }
  }
  
}
