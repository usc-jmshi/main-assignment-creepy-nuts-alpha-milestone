using UnityEngine;

public class DeathWall: MonoBehaviour {
  private float _speed = 2.5f;

  private void Update() {
    transform.position += _speed * Time.deltaTime * Vector3.forward;
  }

  private void OnCollisionEnter(Collision collision) {
    if (collision.gameObject.TryGetComponent(out PlayerController playerController)) {
      GameManager.Instance.Die();
    }
  }
}
