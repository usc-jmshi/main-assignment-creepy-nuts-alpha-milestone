using UnityEngine;
// TODO: remove
using UnityEngine.InputSystem;

public class Hallway: MonoBehaviour {
  public Matrix4x4 PrevConnectionWorldToLocalMat => _prevConnection.worldToLocalMatrix;
  public Hallway Prev { get; set; }
  public Hallway Next { get; set; }

  [SerializeField]
  private Hallway _hallwayPrefab;
  [SerializeField]
  private Transform _prevConnection;
  [SerializeField]
  private Transform _nextConnection;

  private void CreateNext() {
    Next = Instantiate(_hallwayPrefab);

    Matrix4x4 nextWorldMat = Matrix4x4.Translate(_nextConnection.position)
      * Matrix4x4.Rotate(Quaternion.Euler(0, 180, 0))
      * Matrix4x4.Translate(-_nextConnection.position)
      * _nextConnection.localToWorldMatrix
      * Next.PrevConnectionWorldToLocalMat
      * Next.transform.localToWorldMatrix;
    Next.transform.position = nextWorldMat.GetPosition();
    Next.transform.rotation = nextWorldMat.rotation;

    Next.Prev = this;
  }

  private void DestroyPrev() {
    Destroy(Prev.gameObject);
  }

  private void Update() {
    // TODO: remove
    if (Keyboard.current.spaceKey.wasPressedThisFrame) {
      if (Next == null) {
        CreateNext();
      }
    }
  }
}
