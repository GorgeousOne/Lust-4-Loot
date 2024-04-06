using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class ItemLogic : MonoBehaviour {

	public float speed = 0.75f;
	public float bounceVel = 6f;
	public float fallVel = 5f;
	
	public UnityEvent<GameObject> onCannonBallHit;
	private Rigidbody2D rb;
	
	private void OnEnable() {
		rb = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate(){
		rb.velocity = Vector2.right * speed;
	}


	public void Drop() {
		rb.gravityScale = 2;
		rb.velocity = new Vector2(Random.Range(-fallVel, fallVel), bounceVel);
		Destroy(GetComponent<Collider2D>());
		Destroy(gameObject, 5f);
		transform.parent = null;
	}
	
	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag("CannonBall")) {
			onCannonBallHit.Invoke(gameObject);
		}
	}
}