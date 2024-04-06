using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

	public int playerNumber = 1;
	public GameObject bulletPrefab;
	public float speed = 5f;
	public float bulletSpeed = 3f;
	
	private Vector2 inputVel;
	private Rigidbody2D rb;
	
	private void OnEnable() {
		rb = GetComponent<Rigidbody2D>();
	}

	public void OnShoot(){
		int playerFacing = playerNumber == 1 ? 1 : -1;
		GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

		bullet.layer = LayerMask.NameToLayer("Bullet" + playerNumber);
		
		Rigidbody2D billetRb = bullet.GetComponent<Rigidbody2D>();
		billetRb.velocity = Vector2.right * bulletSpeed * playerFacing;

		Debug.Log("TOOT" + playerNumber);
	}

	// private void OnDisable() {
	// 	controls.Player.Disable();
	// }

	public void OnMove( InputAction.CallbackContext context ) {
		inputVel = context.ReadValue<Vector2>();
	}

	private void FixedUpdate() {
		rb.velocity = inputVel * speed;
	}
}