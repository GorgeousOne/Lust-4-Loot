using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ScoreHandler : MonoBehaviour {

	//ui slider to display the score
	public Canvas ingameCanvas;
	public Canvas menuCanvas;
	
	public Slider scoreSlider;
	public GameObject scorePrefab;
	public TMP_Text winnerText;
	public GameObject itemSpawner;
	
	public int scoreRange = 50;
	public float currentScore;
	
	public float moveTime = 1f;
	public float moveRange = 3f;

	public AudioSource soundOnCashOut;
	public AudioSource endGame;
	
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
		
		if (Mathf.Abs(currentScore) >= scoreRange) {
			AnnounceWinner(currentScore > 0);
		}
	}
	
	public void AddPoints(float points, bool isPlayer1) {
		if (points > 1) {
			float multiplier = 1 + 0.5f * (points - 1);
            points *= multiplier;
		}
		
		currentScore += isPlayer1 ? points : -points;
		scoreSlider.value = Remap(currentScore, -scoreRange, scoreRange, 0, 1);
		
		if (points != 0) {
			DisplayPoints((int) points, isPlayer1);
			ChangePos();
			soundOnCashOut.Play();
		}
	}

	private void DisplayPoints(int points, bool isPlayer1) {
		Vector2 textPos = transform.position;
		textPos += (isPlayer1 ? Vector2.left : Vector2.right) + Vector2.up * 0.5f;
		GameObject scoreText = Instantiate(scorePrefab, textPos, Quaternion.identity, ingameCanvas.transform);
		TMP_Text text = scoreText.GetComponent<TMP_Text>(); 
		text.text = "+" + points;
		text.color = isPlayer1 ? Color.red : Color.green;
	}
	
	private void AnnounceWinner(bool isPlayer1) {
		endGame.Play();
		winnerText.text = isPlayer1 ? "Player 1 wins!" : "Player 2 wins!";
		winnerText.color = isPlayer1 ? Color.red : Color.green;
		winnerText.gameObject.SetActive(true);
		menuCanvas.gameObject.SetActive(true);
		
		//disable all movements
		PlayerMovement[] players = FindObjectsOfType<PlayerMovement>();
		foreach (PlayerMovement player in players) {
			player.enabled = false;
		}
		//reset score
		currentScore = 0;
		scoreSlider.value = 0.5f;
		scoreSlider.gameObject.SetActive(false);
		gameObject.SetActive(false);
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