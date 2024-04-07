using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

public class ItemLogic : MonoBehaviour {

	public float speed = 0.75f;
	public float bounceVel = 6f;
	public float fallVel = 5f;
	public float unloadTime = 1f;
	
	public UnityEvent<GameObject> onCannonBallHit;
	private Rigidbody2D rb;
	private Vector2 unloadStart;
	private Transform unloadTarget;
	private float unloadStartTime;
	
	private void OnEnable() {
		rb = GetComponent<Rigidbody2D>();
		rb.velocity = Vector2.right * speed;
	}
	
	public void Update() {
		if (unloadStartTime != 0) {
			float unloadProgress = (Time.time - unloadStartTime) / unloadTime;
			float smooth = 1 - Mathf.Pow(1 - unloadProgress, 3);
			transform.position = Vector2.Lerp(unloadStart, unloadTarget.position, smooth);
		}
	}

	public void Unload(Transform target) {
		unloadStartTime = Time.time;
		unloadTarget = target;
		unloadStart = transform.position;
		Destroy(GetComponent<Collider2D>());
		Destroy(gameObject, unloadTime);
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