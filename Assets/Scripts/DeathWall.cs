using UnityEngine;

public class DeathWall: MonoBehaviour {
    public bool Moving;

  private float _speed = 2.5f;

  private void Update() {
        if (Moving)
        {
            transform.position += _speed * Time.deltaTime * Vector3.forward;
        }
  }

    public void Reset()
    {
        Moving = false;
        transform.position = 25 * Vector3.back;
    }

    private void OnCollisionEnter(Collision collision) {
    if (Moving == false){
      return;
    }
    if (collision.gameObject.TryGetComponent(out PlayerController playerController)) {
      GameManager.Instance.Die();
    }
  }
}
