using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController: MonoBehaviour {
  private const float MoveSpeed = 5f;
  private const float LookSpeed = 0.1f;
  private const float VLookLimit = 60f;

  [SerializeField]
  private Transform _pitchTransform;

  private Rigidbody _rb;
  private Quaternion _currPitchRot;
  private Quaternion _currYawRot;
  private float _currPitch;
  private float _currYaw;

  public void Move(Vector3 dir, float dT) {
    Matrix4x4 localToWorldDir = new(transform.right, Vector4.zero, transform.forward, Vector4.zero);
    Vector3 dR = localToWorldDir * dir.normalized * MoveSpeed * dT;
    _rb.MovePosition(_rb.position + dR);
  }

  public void Look(Vector2 dir) {
    Vector2 dLook = LookSpeed * dir;
    _currPitch -= dLook.y;
    _currYaw += dLook.x;
    _currPitch = Mathf.Clamp(_currPitch, -VLookLimit, VLookLimit);
    _currYaw %= 360f;
    _currPitchRot.eulerAngles = new(_currPitch, _currPitchRot.y, _currPitchRot.z);
    _currYawRot.eulerAngles = new(_currYawRot.x, _currYaw, _currYawRot.z);
    _pitchTransform.localRotation = _currPitchRot;
    transform.localRotation = _currYawRot;
  }

  private void Awake() {
    _rb = GetComponent<Rigidbody>();
    _currPitchRot = _pitchTransform.localRotation;
    _currYawRot = transform.localRotation;
    _currPitch = _currPitchRot.eulerAngles.x;
    _currYaw = _currYawRot.eulerAngles.y;
  }
}
