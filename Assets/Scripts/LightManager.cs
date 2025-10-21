using System;
using UnityEngine;

public class LightManager: MonoBehaviour {
  public static LightManager Instance { get; private set; }

  public LightType LightType { get; private set; }

  public event Action LightTypeSet;

  [SerializeField]
  private Light _playerLight;

public void ToggleLight()
    {
        SetLightType(LightType == LightType.Red ? LightType.Blue : LightType.Red);
        DeathAnalytics.Instance?.RecordColorSwitch();
    }

  private void SetLightType(LightType lightType) {
    switch (lightType) {
      case LightType.Red: {
          _playerLight.color = Color.red;

          break;
        }

      case LightType.Blue: {
          _playerLight.color = Color.blue;

          break;
        }

      default: {
          throw new InvalidOperationException("Invalid light type");
        }
    }

    LightType = lightType;

    LightTypeSet?.Invoke();
  }

  private void Awake() {
    Instance = this;

    SetLightType(LightType);
  }
}
