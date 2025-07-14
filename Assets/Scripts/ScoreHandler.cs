using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ScoreHandler : MonoBehaviour {

	//ui slider to display the score
	[SerializeField] private Canvas ingameCanvas;
	
	[SerializeField] private Slider tugOfWarMeter;
	[SerializeField] private TMP_Text countdownText;
	[SerializeField] private GameObject scorePrefab;
	[SerializeField] private TMP_Text winnerText;
	[SerializeField] private GameObject itemSpawner;

	[SerializeField] private int gameDuration = 90;
	[SerializeField] private int scoreRange = 50;
	
	[SerializeField] private AudioSource soundOnCashOut;
	[SerializeField] private AudioSource endGame;
	[SerializeField] private IslandLogic islandMove;


	public float currentScore;
	private float remainingTime;


	void Start() {
		GameManager.Singleton.OnGameStart.AddListener(SetupIngameUi);
		GameManager.Singleton.OnGameEnd.AddListener(HideInGameUI);
		islandMove.OnLootDeliver.AddListener(OnLootDeliver);

		HideInGameUI();
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

	public void SetupIngameUi() {
		islandMove.gameObject.SetActive(true);
		ingameCanvas.gameObject.SetActive(true);

		currentScore = 0;
		tugOfWarMeter.value = 0.5f;
		remainingTime = gameDuration;

		UpdateTimer();
	}

	private void HideInGameUI() {
		//TODO game ui activate true
		ingameCanvas.gameObject.SetActive(false);
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

		if (Mathf.Abs(currentScore) >= scoreRange) {
			AnnounceWinner(GetWinnerIdx());
		}
	}

	public void AddPoints(int numItems, bool isPlayer1) {
		Debug.Log(numItems);
		int points = numItems;
		//1>1, 2>3, 3>6, 4>9, 5>12, 6>15, 7>18
		if (points > 1) {
			points = 3 * points - 3;
		}

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
		//reset score
		GameManager.Singleton.EndGame();
	}	
}