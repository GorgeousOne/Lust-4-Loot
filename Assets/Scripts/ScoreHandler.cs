using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ScoreHandler : MonoBehaviour {

	//ui slider to display the score
	public Slider scoreSlider;
	
	public int scoreRange = 50;
	public float currentScore;
	
	public float moveTime = 1f;
	public float moveRange = 3f;
	
	private float moveStartTime;
	private Vector2 moveStartPos;
	private Vector2 moveTargetPos;
	
	private void Update() {
		if (Time.time <= moveStartTime + moveTime) {
			float moveProgress = (Time.time - moveStartTime) / moveTime;
			float smooth = 1 - Mathf.Pow(1 - moveProgress, 3);
			transform.position = Vector2.Lerp(moveStartPos, moveTargetPos, smooth);
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		bool isPlayer1 = true;
		
		if (other.gameObject.CompareTag("Player2")) {
			isPlayer1 = false;
		}
		else if (!other.gameObject.CompareTag("Player1")){
			return;
		}
		PlayerCollision player = other.gameObject.GetComponent<PlayerCollision>();
		AddPoints(player.GetItemCount(), isPlayer1);
		player.UnloadItems(transform);
		scoreSlider.value = Remap(currentScore, -scoreRange, scoreRange, 0, 1);
		
		if (currentScore >= scoreRange) {
			Debug.Log("Player 1 wins!");
		} else if (currentScore <= -scoreRange) {
			Debug.Log("Player 2 wins!");
		}
	}
	
	public void AddPoints(float points, bool isPlayer1) {
		if (points >= 3) {
			float multiplier = (points - 2) * 0.5f;
			points *= multiplier;
		}
		currentScore += isPlayer1 ? points : -points;
		scoreSlider.value = Remap(currentScore, -scoreRange, scoreRange, 0, 1);
		
		if (points != 0) {
			ChangePos();
		}
	}

	private void ChangePos() {
		moveStartTime = Time.time;
		moveStartPos = transform.position;
		moveTargetPos = new Vector2(moveStartPos.x, Random.Range(-moveRange, moveRange));
	}
	/**
	 * Remap a value from a range [min, max] to another range [min2, max2]
	 */
	private static float Remap(float x, float min, float max, float min2, float max2) {
		return (x - min) / (max - min) * (max2 - min2) + min2;
	}
	
}