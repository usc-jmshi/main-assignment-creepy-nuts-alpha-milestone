using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler: MonoBehaviour {
  private void Update() {
    // TODO: add to queue to be dequeued and processed in FixedUpdate
    Vector3 moveDir = Vector3.zero;
    if (Keyboard.current.wKey.isPressed) {
      moveDir.z++;
    }
    if (Keyboard.current.aKey.isPressed) {
      moveDir.x--;
    }
    if (Keyboard.current.sKey.isPressed) {
      moveDir.z--;
    }
    if (Keyboard.current.dKey.isPressed) {
      moveDir.x++;
    }
    PlayerController.Instance.Move(moveDir, Time.deltaTime);

    //// TODO: add to queue to be dequeued and processed in FixedUpdate
    //if (Keyboard.current.spaceKey.wasPressedThisFrame) {
    //  PlayerController.Instance.Jump();
    //}

    Vector2 lookDir = Mouse.current.delta.ReadValue();
    PlayerController.Instance.Look(lookDir);

    if (Mouse.current.leftButton.wasPressedThisFrame) {
      LightManager.Instance.PrevLight();
    }

    if (Mouse.current.rightButton.wasPressedThisFrame) {
      LightManager.Instance.NextLight();
    }

    if (Keyboard.current.shiftKey.wasPressedThisFrame) {
      PlayerController.Instance.Dash();
    }
  }

  private void Awake() {
    Cursor.lockState = CursorLockMode.Locked;
  }
}
