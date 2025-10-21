using UnityEngine;

public class Void: MonoBehaviour {
  private void OnTriggerEnter(Collider other) {
    if (other.TryGetComponent(out PlayerController playerController)) {
      GameManager.Instance.Die(DeathCause.FellOffPlatform);
    }
  }
}
