using UnityEngine;
using UnityEngine.Events;

public class IslandLogic : MonoBehaviour {

	public UnityEvent<int> OnLootDeliver;
	public float moveTime = 1f;
	public float moveRange = 3f;

	private float moveStartTime;
	private Vector2 moveStartPos;
	private Vector2 moveTargetPos;


	private void Update() {
		if (IsMoving()) {
			float moveProgress = (Time.time - moveStartTime) / moveTime;
			float smooth = 1 - Mathf.Pow(1 - moveProgress, 3);
			transform.position = Vector2.Lerp(moveStartPos, moveTargetPos, smooth);
		}
	}

	private bool IsMoving() {
		return Time.time <= moveStartTime + moveTime;
	}


	public void ChangePosRng() {
		moveStartTime = Time.time;
		moveStartPos = transform.position;
		moveTargetPos = new Vector2(moveStartPos.x, Random.Range(-moveRange, moveRange));
	}
	
	private void OnTriggerEnter2D(Collider2D other) {
		if (!other.gameObject.tag.Contains("Player")) {
			return;
		}
		PlayerCollision player = other.gameObject.GetComponent<PlayerCollision>();
		int itemCount = player.GetItemCount();

		if (other.gameObject.CompareTag("Player1")) {
			OnLootDeliver.Invoke(itemCount);
		} else if (!other.gameObject.CompareTag("Player2")) {
			OnLootDeliver.Invoke(-itemCount);
		}
		player.UnloadItems(transform);
	}

}
