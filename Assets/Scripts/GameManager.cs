using UnityEngine;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour {

	public static GameManager Singleton { get; private set; }

	[SerializeField] private Canvas titleCanvas;
	[SerializeField] private Canvas pauseCanvas;
	[SerializeField] private GameObject itemSpawner;
	[SerializeField] private GameObject player1;
	[SerializeField] private GameObject player2;
	[SerializeField] private InputActionAsset inputActions;
	[SerializeField] private AudioSource startGame;

	public UnityEvent OnGameStart;
	public UnityEvent OnGameOver;

    public bool IsGamePaused { get; private set; }

	public bool IsGameOver { get; private set; }

	private InputAction pauseAction;

	void Awake() {
		Singleton = this;

		pauseAction = inputActions.FindActionMap("UI").FindAction("TogglePause");
        pauseAction.performed += ctx => TogglePause(); 
	}

    void Start() {
		//just in case
		titleCanvas.gameObject.SetActive(true);   
    }

	void OnEnable() => pauseAction.Enable();
    void OnDisable() => pauseAction.Disable();


    public void StartGame() {
		startGame.Play();

		//hide the menu on start button press
		titleCanvas.gameObject.SetActive(false);
		itemSpawner.SetActive(true);

		player1.transform.position = new Vector3(-7, 0, 0);
		player2.transform.position = new Vector3(7, 0, 0);

		IsGameOver = false;
		OnGameStart.Invoke();
	}

	public void EndGame() {		
		IsGameOver = true;
		titleCanvas.gameObject.SetActive(true);
		itemSpawner.SetActive(false);
		OnGameOver.Invoke();
	}
 
    void TogglePause() {
		if (IsGameOver) {
			return;
		}
        IsGamePaused = !IsGamePaused;
        Time.timeScale = IsGamePaused ? 0 : 1;
		pauseCanvas.gameObject.SetActive(IsGamePaused);
        Debug.Log("Game " + (IsGamePaused ? "Paused" : "Unpaused"));
    }
}
