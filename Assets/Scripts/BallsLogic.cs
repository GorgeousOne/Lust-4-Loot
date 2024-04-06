using System;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;

public class BallsLogic : MonoBehaviour {
	
	[FormerlySerializedAs("bounciness")] public float bounceVel = 3f;
	Rigidbody2D rb;

	private void OnEnable() {
		rb = GetComponent<Rigidbody2D>();
	}
	
	private void Drop() {
		Destroy(GetComponent<Collider2D>());
		rb.gravityScale = 3;
		rb.velocity = Vector2.up * bounceVel;
		Destroy(gameObject, 5f);
	}
	
	private void OnCollisionEnter2D(Collision2D collision) {
		Drop();
	}
}