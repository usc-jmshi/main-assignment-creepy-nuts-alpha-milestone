using Random = UnityEngine.Random;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager: MonoBehaviour {
  public static PlatformManager Instance { get; private set; }

  private const int Lead = 20;
  private const int NumLanes = 3;
  private const float LaneGap = 0f;

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
    platform.transform.position = new(GetNextXPos(platform), GetNextYPos(), GetNextZPos(platform));
    platform.transform.SetParent(transform);
    platform.name = $"Platform{platform.Index}";
    _platforms.Add(platform);

        //Temporary code for collectible management
        if (Random.value <= 0.2f)
            CollectibleManager.Instance.CreateCollectible(0, platform.transform.position + Vector3.up * 0.5f);
  }

  private float GetNextXPos(Platform platform) {
    float laneIndex = Random.Range(0, NumLanes) - (NumLanes / 2 - (NumLanes % 2 == 0 ? 0.5f : 0));
    return laneIndex * (platform.transform.lossyScale.x + LaneGap);
  }

  private float GetNextYPos() {
    return 0f;
  }

  private float GetNextZPos(Platform platform) {
    if (_platforms.Count == 0) {
      return 0f;
    }

    return _platforms[_platforms.Count - 1].transform.position.z + 0.5f * (_platforms[_platforms.Count - 1].transform.lossyScale.z + platform.transform.lossyScale.z);
  }

  private void Start() {
    CreatePlatforms(-1);
  }

  private void Awake() {
    Instance = this;
  }
}
