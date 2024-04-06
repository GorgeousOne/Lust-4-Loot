using System;
using UnityEditor.SceneManagement;
using UnityEngine;

public class BallsLogic : MonoBehaviour {
	
	public float bounciness = 3f;
	Rigidbody2D rb;

	private void OnEnable() {
		rb = GetComponent<Rigidbody2D>();
	}
	
	private void Drop() {
		Destroy(GetComponent<Collider2D>());
		rb.gravityScale = 3;
		rb.velocity = Vector2.up * bounciness;
		Destroy(gameObject, 5f);
	}
	
	private void OnCollisionEnter2D(Collision2D collision) {
		Drop();
	}
}