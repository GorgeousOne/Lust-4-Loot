using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UiManager : MonoBehaviour {

	public static UiManager Instance { get; private set; }

	[SerializeField] private GameObject titleUi;
	[SerializeField] private GameObject pauseUi;
	[SerializeField] private GameObject settingsUi;


	[SerializeField] private InputActionAsset inputActions;

	private Stack<GameObject> uiStack = new Stack<GameObject>();
	private GameObject activeUi;
	private InputAction pauseAction;

	void Awake() {
		Instance = this;
		pauseAction = inputActions.FindActionMap("UI").FindAction("TogglePause");
		pauseAction.performed += ctx => OnTogglePause();
	}

	void Start() {
		GameManager.Instance.OnGameStart.AddListener(OnGameStart);
		GameManager.Instance.OnGameOver.AddListener(OnGameOver);

		//just in case
		titleUi.SetActive(true);
		activeUi = titleUi;
    }	


    void OnEnable() => pauseAction.Enable();
	void OnDisable() => pauseAction.Disable();

	private void OnGameStart() {
		titleUi.SetActive(false);
		activeUi = null;
	}

	private void OnGameOver() {
		titleUi.SetActive(true);
		activeUi = titleUi;
	}

	public void OpenSettings() {
		if (activeUi != null) {
			activeUi.SetActive(false);
			uiStack.Push(activeUi);
		}
		settingsUi.SetActive(true);
		activeUi = settingsUi;
	}

	public void CloseSettings() {
		settingsUi.SetActive(false);
		activeUi = uiStack.Pop();
		activeUi.SetActive(true);
	}

	public void OnTogglePause() {
		if (GameManager.Instance.IsGameOver) {
			return;
		}
		bool isGamePaused = GameManager.Instance.TogglePause();
		pauseUi.gameObject.SetActive(isGamePaused);
		activeUi = isGamePaused ? pauseUi : null;
 		Debug.Log("Game " + (isGamePaused ? "Paused" : "Unpaused"));
	}

	void OnBackPerformed() {
		if (activeUi == settingsUi) {
			CloseSettings();
		} else if (activeUi == pauseUi) {
			OnTogglePause();
		}
	}
}
