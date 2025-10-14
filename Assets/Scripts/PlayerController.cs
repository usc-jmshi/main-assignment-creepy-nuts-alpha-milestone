using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController: MonoBehaviour {
  public static PlayerController Instance { get; private set; }

  private const float MoveSpeed = 5f;
  private const float LookSpeed = 0.1f;
  //private const float JumpSpeed = 5f;
  private const float VLookLimit = 60f;
  private const float DashDuration = 0.1f;
  private const float DashDistance = 4f;
  public GameObject clone;

  [SerializeField]
  private Transform _pitchTransform;

  private Rigidbody _rb;
  private Quaternion _currPitchRot;
  private Quaternion _currYawRot;
  private Vector3 _currPitchEulerAngles;
  private Vector3 _currYawEulerAngles;
  private Coroutine _dashCoroutine;
  //private bool _grounded;

  public void Move(Vector3 dir, float dT) {
    Matrix4x4 localToWorldDir = new(transform.right, Vector4.zero, transform.forward, Vector4.zero);
    Vector3 dR = localToWorldDir * dir.normalized * MoveSpeed * dT;
    _rb.MovePosition(_rb.position + dR);
  }

  public void Look(Vector2 dir) {
    Vector2 dLook = LookSpeed * dir;
    _currPitchEulerAngles.x -= dLook.y;
    _currYawEulerAngles.y += dLook.x;
    _currPitchEulerAngles.x = Mathf.Clamp(_currPitchEulerAngles.x, -VLookLimit, VLookLimit);
    _currYawEulerAngles.y %= 360f;
    _currPitchRot.eulerAngles = _currPitchEulerAngles;
    _currYawRot.eulerAngles = _currYawEulerAngles;
    _pitchTransform.localRotation = _currPitchRot;
    transform.localRotation = _currYawRot;
  }

  // TODO: add cooldown or ammunition replenished by platforms?
  public void Dash() {
    if (_dashCoroutine != null) {
      return;
    }

    _dashCoroutine = StartCoroutine(DashCoroutine());
  }

  private IEnumerator DashCoroutine() {
    _rb.linearVelocity = DashDistance / DashDuration * transform.forward;
    yield return new WaitForSeconds(DashDuration);
    _rb.linearVelocity = Vector3.zero;
    _dashCoroutine = null;
  }

  //public void Jump() {
  //  if (_grounded) {
  //    _rb.AddForce(JumpSpeed * transform.up, ForceMode.VelocityChange);
  //    _grounded = false;
  //  }
  //}

  //// TODO: Add parameter where you check if the player has enough mana to create a copy. Rn just creates
  public void Copy()
  {
    Vector3 clonePos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
    Instantiate(clone, clonePos, Quaternion.identity);
  }

  private void Awake()
  {
    Instance = this;

    _rb = GetComponent<Rigidbody>();
    _currPitchRot = _pitchTransform.localRotation;
    _currYawRot = transform.localRotation;
    _currPitchEulerAngles = _currPitchRot.eulerAngles;
    _currYawEulerAngles = _currYawRot.eulerAngles;
  }

  //// TODO: fix falling off without jumping case 
  //private void OnCollisionEnter(Collision collision) {
  //  if (!_grounded) {
  //    _grounded = Vector3.Dot(collision.GetContact(0).normal, transform.up) > 0;
  //  }
  //}
}
