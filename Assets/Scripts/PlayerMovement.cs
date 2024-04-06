using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
	//input fields

	public InputActionAsset inputAsset;
	public String actionMapName;
	
	public GameObject bulletPrefab;
	public float speed = 5f;
	public float bulletSpeed = 3f;
	
	private InputActionMap controls;
	private InputAction move;
	private Vector2 inputVel;
	private Rigidbody2D rb;
	
	private void OnEnable() {
		rb = GetComponent<Rigidbody2D>();
		controls = inputAsset.FindActionMap(actionMapName);
		Debug.Log("found map " + actionMapName + " " + controls);
		move = controls["Move"];
		controls["Shoot"].performed += OnShoot;
		
		controls.Enable();
	}

	private void OnDisable() {
		controls.Disable();
	}

	private void OnShoot(InputAction.CallbackContext context){
		GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
		bullet.layer = LayerMask.NameToLayer("OurBullet");
		Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
		rb.velocity = Vector2.right * bulletSpeed;
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