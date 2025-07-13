using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ScoreHandler : MonoBehaviour {

	//ui slider to display the score
	public Canvas ingameCanvas;
	public Canvas menuCanvas;
	
	public Slider tugOfWarMeter;
	public TMP_Text countdownText;
	public GameObject scorePrefab;
	public TMP_Text winnerText;
	public GameObject itemSpawner;

	public int gameDuration = 90;
	public int scoreRange = 50;
	
	public AudioSource soundOnCashOut;
	public AudioSource endGame;


	public float currentScore;
	private float remainingTime;

	public IslandLogic islandMove;


	void Start() {
		Debug.Log("wass goin on " + GameManager.Singleton);
		GameManager.Singleton.OnGameStart.AddListener(SetupScores);
		GameManager.Singleton.OnGameEnd.AddListener(HideScores);
		islandMove.OnLootDeliver.AddListener(OnLootDeliver);
    }

	void Update() {
		if (!GameManager.Singleton.IsGameRunning) {
			return;
		}

		if (remainingTime < 0) {
			countdownText.text = "0:00";
			AnnounceWinner(GetWinnerIdx());
			return;
		}
		remainingTime -= Time.deltaTime;
		UpdateTimer();
    }

	public void SetupScores() {
		tugOfWarMeter.gameObject.SetActive(true);
		islandMove.gameObject.SetActive(true);


		Debug.Log("AYO ANYONE HOME?");
		currentScore = 0;
		tugOfWarMeter.value = 0.5f;
		remainingTime = gameDuration;
		UpdateTimer();
	}

	private void HideScores() {
		tugOfWarMeter.gameObject.SetActive(false);
		islandMove.gameObject.SetActive(false);

	}

	private void UpdateTimer() {
		int minutes = Mathf.FloorToInt(remainingTime / 60);
		int seconds = Mathf.FloorToInt(remainingTime % 60);
		countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
	}

	private int GetWinnerIdx() {
		if (currentScore == 0) {
			return 0;
		}
		return currentScore > 0 ? 1 : 2;
	}

	private void OnLootDeliver(int numItems) {
		AddPoints(Mathf.Abs(numItems), numItems > 0);
		tugOfWarMeter.value = Mathf.InverseLerp(-scoreRange, scoreRange, currentScore);

		if (Mathf.Abs(currentScore) >= scoreRange) {
			AnnounceWinner(GetWinnerIdx());
		}
	}

	public void AddPoints(int points, bool isPlayer1) {
		//apparently this is a triangular number progression
		//1>1, 2>3, 3>6, 4>10, 5>15, 6>21, 7>28
		points = (points * (points + 1)) / 2;

		currentScore += isPlayer1 ? points : -points;
		tugOfWarMeter.value = Mathf.InverseLerp(-scoreRange, scoreRange, currentScore);

		if (points != 0) {
			DisplayPoints(points, isPlayer1);
			islandMove.ChangePosRng();
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

	private void AnnounceWinner(int playerIdx) {
		endGame.Play();

		if (playerIdx == 1 || playerIdx == 2) {
			winnerText.text = playerIdx == 1 ? "Player 1 wins!" : "Player 2 wins!";
			winnerText.color = playerIdx == 1 ? Color.red : Color.green;
		} else {
			winnerText.text = "It's a Draw!";
			winnerText.color = Color.blue;
		}
		winnerText.gameObject.SetActive(true);
		menuCanvas.gameObject.SetActive(true);

		//reset score
		tugOfWarMeter.gameObject.SetActive(false);
		countdownText.gameObject.SetActive(false);
		gameObject.SetActive(false);
		GameManager.Singleton.EndGame();
	}	
}