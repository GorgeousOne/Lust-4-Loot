using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using System.Collections.Generic;

public class ItemLogic : Droppable {

	public float floatSpeed = 0.75f;
	public float unloadTime = 1f;

	public List<Sprite> icons;

	public UnityEvent<GameObject> OnCannonBallHit;
	private Rigidbody2D rb;
	private Vector2 unloadStart;
	private Transform unloadTarget;
	private float unloadStartTime;

	private void OnEnable() {
		rb = GetComponent<Rigidbody2D>();
		rb.linearVelocity = Vector2.right * floatSpeed;

		int rand = Random.Range(0, icons.Count );
		GetComponent<SpriteRenderer>().sprite = icons[rand];
	}

	protected new void Update() {
		base.Update();

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

	public new void Drop() {
		base.Drop();
		transform.parent = null;
		Destroy(GetComponent<Collider2D>());
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag("CannonBall")) {
			//let the ship handle the dropping;
			OnCannonBallHit.Invoke(gameObject);
		}
		//remove item on contact with game bounds
		else if (other.gameObject.layer == LayerMask.NameToLayer("Default")) {
			Destroy(gameObject);
		}
	}
}