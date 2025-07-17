using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Droppable : MonoBehaviour {

	[SerializeField] GameObject splashPrefab;
	[SerializeField] float yVel = 3f;
	[SerializeField] float xVelRange = 5f;
	[SerializeField] float splashOffset = -1f;
	[SerializeField][Min(0)] float splashRange = 1f;
	[SerializeField] float gravity = 2f;

	private Rigidbody2D rigid;
	private float waterLevel;
	private bool isDropping;

    void Start() {
		rigid = GetComponent<Rigidbody2D>();
    }

    protected void Update() {
		if (!isDropping) {
			return;
		}
		if (transform.position.y <= waterLevel) {
			Instantiate(splashPrefab, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}

	public void Drop() {
		rigid.gravityScale = gravity;
		rigid.linearVelocity = new Vector2(Random.Range(-xVelRange, xVelRange), yVel);
		waterLevel = transform.position.y + splashOffset + Random.Range(-splashRange, splashRange);
		isDropping = true;
	}
}