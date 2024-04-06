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
		rb.velocity = Vector2.right * speed;
	}

	public void Drop() {
		rb.gravityScale = 3;
		rb.velocity = new Vector2(Random.Range(-fallVel, fallVel), bounceVel);

		transform.parent = null;
		Destroy(GetComponent<Collider2D>());
		Destroy(gameObject, 5f);
	}
	
	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag("CannonBall")) {
			onCannonBallHit.Invoke(gameObject);
		}
		else if (other.gameObject.layer == LayerMask.NameToLayer("Default")) {
			Destroy(gameObject);		
		}
	}
}