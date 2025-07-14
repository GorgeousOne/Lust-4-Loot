using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour {

	public static GameManager Instance { get; private set; }

	[SerializeField] private GameObject itemSpawner;
	[SerializeField] private GameObject player1;
	[SerializeField] private GameObject player2;
	[SerializeField] private AudioSource startGame;

	public UnityEvent OnGameStart;
	public UnityEvent OnGameOver;

	public bool IsGamePaused { get; private set; }

	public bool IsGameOver { get; private set; }


	void Awake() {
		Instance = this;
	}

	public void StartGame() {
		startGame.Play();
		itemSpawner.SetActive(true);

		player1.transform.position = new Vector3(-7, 0, 0);
		player2.transform.position = new Vector3(7, 0, 0);

		IsGameOver = false;
		OnGameStart.Invoke();
	}

	public void EndGame() {
		IsGameOver = true;
		itemSpawner.SetActive(false);
		OnGameOver.Invoke();
	}

	public bool TogglePause() {
		IsGamePaused = !IsGamePaused;
		Time.timeScale = IsGamePaused ? 0 : 1;
		return IsGamePaused;
	}
}
