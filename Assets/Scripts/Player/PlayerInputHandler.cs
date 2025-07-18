using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour {

	private PlayerInput input;
	private PlayerMover move;

	void Awake() {
		input = GetComponent<PlayerInput>();
		var movers = FindObjectsByType<PlayerMover>(FindObjectsSortMode.None);
		move = movers.FirstOrDefault(m => m.GetPlayerIndex() == input.playerIndex + 1);
		Debug.Log("I'll fucking kill you " + input.playerIndex);
	}

	public void OnMove(InputAction.CallbackContext context) {
		move.SetMoveInputVec(context.ReadValue<Vector2>());
	}

	public void OnShoot(InputAction.CallbackContext context) {
		move.TriggerShootInput();
	}
}
