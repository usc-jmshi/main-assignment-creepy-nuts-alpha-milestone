using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController: MonoBehaviour {
  public static PlayerController Instance { get; private set; }

    public int DashesLeft => _dashesLeft;
    public float DashRefillPercentage => Mathf.Clamp(_dashRefillTimer / DashRefillDuration, 0, 1);

  private const float MoveSpeed = 5f;
  private const float LookSpeed = 0.1f;
  //private const float JumpSpeed = 5f;
  private const float VLookLimit = 60f;
  private const float DashDuration = 0.1f;
  private const float DashDistance = 4f;
    public const int MaxDashes = 3;
    private const float DashRefillDuration = 2f;

  [SerializeField]
  private Transform _pitchTransform;

  private Rigidbody _rb;
  private Quaternion _currPitchRot;
  private Quaternion _currYawRot;
  private Vector3 _currPitchEulerAngles;
  private Vector3 _currYawEulerAngles;
  private Coroutine _dashCoroutine;
    //private bool _grounded;
    private int _dashesLeft = MaxDashes;
    private Coroutine _dashRefillCoroutine;
    private float _dashRefillTimer;
    private bool _canDash = true;

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
    if (!_canDash || _dashCoroutine != null || _dashesLeft == 0) {
      return;
    }

    _dashesLeft--;
        _canDash = false;
    _dashCoroutine = StartCoroutine(DashCoroutine());
     
        if (_dashRefillCoroutine != null)
        {
            StopCoroutine(_dashRefillCoroutine);
        }
        _dashRefillCoroutine = StartCoroutine(DashRefillCoroutine());
    }

  private IEnumerator DashCoroutine() {
    _rb.linearVelocity = DashDistance / DashDuration * transform.forward;
    yield return new WaitForSeconds(DashDuration);
    _rb.linearVelocity = Vector3.zero;
    _dashCoroutine = null;
  }

  private IEnumerator DashRefillCoroutine()
    {
        while (_dashesLeft < MaxDashes)
        {
            _dashRefillTimer = 0;
            while (_dashRefillTimer < DashRefillDuration)
            {
                yield return null;
                _dashRefillTimer += Time.deltaTime;
            }
            GiveDash(1);
        }

        _dashRefillCoroutine = null;
    }

    public void GiveDash(int dashes)
    {
        _dashesLeft = Mathf.Min(MaxDashes, _dashesLeft + dashes);
    }

  //public void Jump() {
  //  if (_grounded) {
  //    _rb.AddForce(JumpSpeed * transform.up, ForceMode.VelocityChange);
  //    _grounded = false;
  //  }
  //}

  private void Awake() {
    Instance = this;

    _rb = GetComponent<Rigidbody>();
    _currPitchRot = _pitchTransform.localRotation;
    _currYawRot = transform.localRotation;
    _currPitchEulerAngles = _currPitchRot.eulerAngles;
    _currYawEulerAngles = _currYawRot.eulerAngles;
  }

    private void OnCollisionStay(Collision collision)
    {
        if (!_canDash && _dashCoroutine == null)
        {
            _canDash = true;
        }
    }

    //// TODO: fix falling off without jumping case 
    //private void OnCollisionEnter(Collision collision) {
    //  if (!_grounded) {
    //    _grounded = Vector3.Dot(collision.GetContact(0).normal, transform.up) > 0;
    //  }
    //}
}
