using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
public class PlayerInputHandler: MonoBehaviour {
  private PlayerController _controller;

  private void Awake() {
    _controller = GetComponent<PlayerController>();
  }

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
    _controller.Move(moveDir, Time.deltaTime);

    Vector2 lookDir = Mouse.current.delta.ReadValue();
    _controller.Look(lookDir);
  }
}
