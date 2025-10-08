using UnityEngine;

public class Void: MonoBehaviour {
  [SerializeField]
  private Transform startTransform;

  private void OnTriggerEnter(Collider other) {
    if (other.TryGetComponent(out PlayerController playerController)) {
      playerController.GetComponent<Rigidbody>().position = startTransform.position;
    }
  }
}
