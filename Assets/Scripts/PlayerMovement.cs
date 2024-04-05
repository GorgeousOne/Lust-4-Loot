using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
	//input fields
	private PlayerControls controls;
	private InputAction move;
	private Vector2 inputVel;
	
	//movement fields
	private Rigidbody2D rb;
	[SerializeField] private float speed = 5f;

	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
		controls = new PlayerControls();
	}

	private void OnEnable() {
		move = controls.Player.Move;
		controls.Player.Enable();
	}

	// private void OnDisable() {
	// 	controls.Player.Disable();
	// }

	private void Update() {
		inputVel = move.ReadValue<Vector2>();
	}

	private void FixedUpdate() {
		rb.velocity = inputVel * speed;
	}
}