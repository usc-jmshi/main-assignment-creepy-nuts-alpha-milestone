using UnityEngine;

public class GameManager: MonoBehaviour {
  public static GameManager Instance { get; private set; }

  [SerializeField]
  private DeathWall _deathWall;
  [SerializeField]
  private PlayerController _playerController;
  [SerializeField]
  private Transform _startTransform;

  public void Die() {
    NotificationUI.Instance.Notify("DEAD", Color.red);

    _playerController.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
    _playerController.transform.position = _startTransform.position;

        _playerController.GiveDash(PlayerController.MaxDashes);

        _deathWall.Reset();
  }

  private void Awake() {
    Instance = this;
  }
}
