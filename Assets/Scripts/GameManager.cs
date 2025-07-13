using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {

	public static GameManager Singleton { get; private set; }

	public Canvas menuCanvas;
	public GameObject itemSpawner;
	public GameObject player1;
	public GameObject player2;
	public AudioSource startGame;

	public UnityEvent OnGameStart;
	public UnityEvent OnGameEnd;

	public bool IsGameRunning { get; private set; }

	void Awake() {
		Singleton = this;
	}

	public void StartGame() {
		startGame.Play();

		//hide the menu on start button press
		menuCanvas.gameObject.SetActive(false);
		itemSpawner.SetActive(true);

		player1.transform.position = new Vector3(-7, 0, 0);
		player2.transform.position = new Vector3(7, 0, 0);

		//find all PlayerMovement scripts and enable them
		PlayerMovement[] players = FindObjectsOfType<PlayerMovement>();
		foreach (PlayerMovement player in players) {
			player.enabled = true;
		}
		PlayerCollision[] players1 = FindObjectsOfType<PlayerCollision>();
		foreach (PlayerCollision player1 in players1) {
			player1.TakeDamage();
		}

		IsGameRunning = true;
		OnGameStart.Invoke();
	}

	public void EndGame() {
		//disable all movements
		PlayerMovement[] players = FindObjectsOfType<PlayerMovement>();
		foreach (PlayerMovement player in players) {
			player.enabled = false;
		}
		
		IsGameRunning = false;
		OnGameEnd.Invoke();
	}
}