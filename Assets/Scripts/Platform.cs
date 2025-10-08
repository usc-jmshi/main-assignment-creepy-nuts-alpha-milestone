using UnityEngine;

public class Platform: MonoBehaviour {
  public int Index { private get; set; }

  [SerializeField]
  private MeshRenderer _mr;
  [SerializeField]
  private Material _completedMat;

  private LightType _lightType;
  private bool _complete;

  public void SetLightType(LightType lightType) {
    _lightType = lightType;

    OnLightSet();
  }

  private void OnLightSet() {
    if (_complete) {
      _mr.enabled = true;
      _mr.material = _completedMat;
      LightManager.Instance.LightSet -= OnLightSet;

      return;
    }

    _mr.enabled = _lightType == LightManager.Instance.LightType;
  }

  private void Awake() {
    LightManager.Instance.LightSet += OnLightSet;
  }

  private void OnDestroy() {
    LightManager.Instance.LightSet -= OnLightSet;
  }

  private void OnCollisionEnter(Collision collision) {
    if (!collision.gameObject.TryGetComponent(out PlayerController _)) {
      return;
    }


    if (Vector3.Dot(collision.GetContact(0).normal, -transform.up) > 0) {
      _complete = true;
      OnLightSet();

      PlatformManager.Instance.CreatePlatforms(Index);
    }
  }
}
