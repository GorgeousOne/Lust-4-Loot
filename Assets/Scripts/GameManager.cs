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

    void Start() {
		//just in case
		menuCanvas.gameObject.SetActive(true);   
    }

    public void StartGame() {
		startGame.Play();

		//hide the menu on start button press
		menuCanvas.gameObject.SetActive(false);
		itemSpawner.SetActive(true);

		player1.transform.position = new Vector3(-7, 0, 0);
		player2.transform.position = new Vector3(7, 0, 0);

		IsGameRunning = true;
		OnGameStart.Invoke();
	}

	public void EndGame() {		
		IsGameRunning = false;
		menuCanvas.gameObject.SetActive(true);
		itemSpawner.SetActive(false);
		OnGameEnd.Invoke();
	}
}