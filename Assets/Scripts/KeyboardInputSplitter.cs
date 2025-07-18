using UnityEngine;

public class KeyboardInputSplitter : MonoBehaviour {

	public PlayerMover player1;
	public PlayerMover player2;
	private SharedKeyboardInput input;

	private Vector2 pMove1;
	private Vector2 pMove2;

	void Awake() {
		input = new SharedKeyboardInput();
		input.SharedKeyboard.Enable();
		input.SharedKeyboard.Shoot1.performed += ctx => player1.TriggerShootInput();
		input.SharedKeyboard.Shoot2.performed += ctx => player2.TriggerShootInput();
	}

	void Update() {
		Vector2 move1 = input.SharedKeyboard.Move1.ReadValue<Vector2>();
		Vector2 move2 = input.SharedKeyboard.Move2.ReadValue<Vector2>();

		if (move1 != Vector2.zero || pMove1 != Vector2.zero) {
			player1.SetMoveInputVec(move1);
		}
		if (move2 != Vector2.zero || pMove2 != Vector2.zero) {
			player2.SetMoveInputVec(move2);
		}
		pMove1 = move1;
		pMove2 = move2;
	}
}
