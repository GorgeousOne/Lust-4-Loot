using UnityEngine;
using UnityEngine.Serialization;

public class BallsLogic : MonoBehaviour {
	
	[FormerlySerializedAs("bounciness")] public float bounceVel = 3f;
	Rigidbody2D rb;

	private void OnEnable() {
		rb = GetComponent<Rigidbody2D>();
	}
	
	private void Drop() {
		rb.gravityScale = 2;
		rb.velocity = Vector2.up * bounceVel;
		Destroy(GetComponent<Collider2D>());
		Destroy(gameObject, 5f);
	}
	
	private void OnCollisionEnter2D(Collision2D collision) {
		Drop();
	}
}