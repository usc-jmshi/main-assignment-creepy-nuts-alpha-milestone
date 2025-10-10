using System;
using UnityEngine;

public class LightManager: MonoBehaviour {
  public static LightManager Instance { get; private set; }

  public LightType LightType { get; private set; }

  public event Action LightTypeSet;

  [SerializeField]
  private Light _playerLight;

  public void PrevLight() {
    int numLightTypes = Enum.GetValues(typeof(LightType)).Length;
    LightType prevLight = (LightType) (((int) LightType - 1 + numLightTypes) % numLightTypes);
    SetLightType(prevLight);
  }

  public void NextLight() {
    LightType nextLight = (LightType) (((int) LightType + 1) % Enum.GetValues(typeof(LightType)).Length);
    SetLightType(nextLight);
  }

  private void SetLightType(LightType lightType) {
    switch (lightType) {
      case LightType.Red: {
          _playerLight.color = Color.red;

          break;
        }

      case LightType.Green: {
          _playerLight.color = Color.green;

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
