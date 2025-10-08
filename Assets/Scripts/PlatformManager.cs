using Random = UnityEngine.Random;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager: MonoBehaviour {
  public static PlatformManager Instance { get; private set; }

  private const float Gap = 2.5f;
  private const int Lead = 20;

  [SerializeField]
  private Platform _platformPrefab;

  private readonly List<Platform> _platforms = new();

  private int _nextIndex;

  public void CreatePlatforms(int currIndex) {
    for (int i = _nextIndex - currIndex - 1; i < Lead; i++) {
      CreatePlatform();
    }
  }

  private void CreatePlatform() {
    Platform platform = Instantiate(_platformPrefab);
    platform.SetLightType((LightType) Random.Range(0, Enum.GetValues(typeof(LightType)).Length));
    platform.Index = _nextIndex;
    _nextIndex++;
    if (_platforms.Count == 0) {
      platform.transform.position = Vector3.zero;
    } else {
      platform.transform.position = _platforms[_platforms.Count - 1].transform.position
        + (0.5f * (_platforms[_platforms.Count - 1].transform.lossyScale.z + platform.transform.lossyScale.z) + Gap) * Vector3.forward;
    }
    _platforms.Add(platform);
  }

  private void Start() {
    CreatePlatforms(-1);
  }

  private void Awake() {
    Instance = this;
  }
}
