using System;
using UnityEngine;

public class BallsLogic : MonoBehaviour {
	
	// private Rigidbody2D rb;
	
	// private void OnEnable() {
	// 	rb = GetComponent<Rigidbody2D>();
	// 	rb.velocity = Vector2.left * 3;
	// }

	private void OnCollisionEnter2D(Collision2D collision) {
		Destroy(gameObject, 0.1f);
	}
}