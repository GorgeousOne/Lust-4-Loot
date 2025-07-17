using UnityEngine;
using UnityEngine.Events;

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
		IsGameOver = true;
	}

    void Start() {
		SoundManager.PlayMusic(SoundType.THEME);
    }

    public void StartGame() {
		SoundManager.PlaySfx(SoundType.ARGH);
		itemSpawner.SetActive(true);

		player1.transform.position = new Vector3(-7, 0, 0);
		player2.transform.position = new Vector3(7, 0, 0);

		IsGameOver = false;
		OnGameStart.Invoke();
	}

	public void EndGame() {
		SoundManager.PlaySfx(SoundType.FANFARE);
		UnpauseGame();
		IsGameOver = true;
		itemSpawner.SetActive(false);
		OnGameOver.Invoke();
	}

	public void PauseGame() {
		IsGamePaused = true;
		Time.timeScale = 0;
	}

	public void UnpauseGame() {
		IsGamePaused = false;
		Time.timeScale = 1;
	}

	public static void ExitGame() {
	    Application.Quit();
	}
}
