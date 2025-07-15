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
	private InputActionMap gameUiMap;
	private InputActionMap pauseUiMap;

	void Awake() {
		Instance = this;
		gameUiMap = inputActions.FindActionMap("GameUI");
		pauseUiMap = inputActions.FindActionMap("PauseUI");
		
		gameUiMap.FindAction("Pause").performed += ctx => OnPausePerformed();
		pauseUiMap.FindAction("Unpause").performed += ctx => OnUnpausePerformed();
		pauseUiMap.FindAction("Back").performed += ctx => OnBackPerformed();
	}

	void Start() {
		GameManager.Instance.OnGameStart.AddListener(OnGameStart);
		GameManager.Instance.OnGameOver.AddListener(OnGameOver);
		OpenUi(titleUi);
    }

    void OnEnable() {
		pauseUiMap.Enable();
    }

    private void OnGameStart() {
		CloseUi();
		gameUiMap.Enable();
	}

	private void OnGameOver() {
		OpenUi(titleUi);
		gameUiMap.Disable();
		pauseUiMap.Enable();
	}

	public void CloseUi() {
		if (activeUi == pauseUi) {
			GameManager.Instance.UnpauseGame();
		}
		activeUi.SetActive(false);

		if (uiStack.Count > 0) {
			activeUi = uiStack.Pop();
			activeUi.SetActive(true);
		}
	}

	public void OnPausePerformed() {
		//only pause ingame
		if (GameManager.Instance.IsGameOver) {
			return;
		}
		gameUiMap.Disable();
		pauseUiMap.Enable();

		GameManager.Instance.PauseGame();
		OpenUi(pauseUi);
	}

	public void OnUnpausePerformed() {
		//don't close the title screen
		if (GameManager.Instance.IsGameOver) {
			return;
		}
		GameManager.Instance.UnpauseGame();
		pauseUiMap.Disable();
		gameUiMap.Enable();
		CloseUi();
	}

	public void OnBackPerformed() {
		//don't close the title screen
		if (GameManager.Instance.IsGameOver && uiStack.Count == 0) {
			return;
		}
		//unpause when pause menu is closed
		if (activeUi == pauseUi) {
			OnUnpausePerformed();
		} else {
			CloseUi();
		}
	}

	public void OpenUi(GameObject ui) {
		uiStack.Clear();
		activeUi?.SetActive(false);
		ui.SetActive(true);
		activeUi = ui;
	}

	public void OpenUiNested(GameObject ui) {
		uiStack.Push(activeUi);
		activeUi.SetActive(false);
		ui.SetActive(true);
		activeUi = ui;
	}
}
