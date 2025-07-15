using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardInputSplitter : MonoBehaviour {

	public PlayerMove player1;
	public PlayerMove player2;
	private SharedKeyboardInput input;

	void Awake() {
		input = new SharedKeyboardInput();
		input.Enable();
	}

	private Vector2 pMove1;
	private Vector2 pMove2;

	void Update() {
		Vector2 move1 = input.SharedKeyboard.Move1.ReadValue<Vector2>();
		Vector2 move2 = input.SharedKeyboard.Move2.ReadValue<Vector2>();
		bool shoot1 = input.SharedKeyboard.Shoot1.triggered;
		bool shoot2 = input.SharedKeyboard.Shoot2.triggered;

		if (move1 != Vector2.zero || pMove1 != Vector2.zero) {
			player1.Velocitate(move1);
		}
		if (move2 != Vector2.zero || pMove2 != Vector2.zero) {
			player2.Velocitate(move2);
		}
		pMove1 = move1;
		pMove2 = move2;

		if (shoot1) {
			player1.OnShoot();
		}
		if (shoot2) {
			player2.OnShoot();
		}
	}
}
