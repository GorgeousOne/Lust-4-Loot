using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

	public static UiManager Instance { get; private set; }

	[SerializeField] private GameObject titleUi;
	[SerializeField] private GameObject pauseUi;
	[SerializeField] private GameObject settingsUi;


	[SerializeField] private InputActionAsset inputActions;

	private Stack<GameObject> uiStack = new Stack<GameObject>();
	private GameObject activeUi;
	private InputAction pauseAction;
	private InputAction backAction;

	void Awake() {
		Instance = this;
		pauseAction = inputActions.FindActionMap("UI").FindAction("Pause");
		backAction = inputActions.FindActionMap("UI").FindAction("Back");
		pauseAction.performed += ctx => OnPausePerformed();
		backAction.performed += ctx => CloseUi();
	}

	void Start() {
		GameManager.Instance.OnGameStart.AddListener(OnGameStart);
		GameManager.Instance.OnGameOver.AddListener(OnGameOver);
		OpenUi(titleUi);
    }	

    void OnEnable() => pauseAction.Enable();
	void OnDisable() => pauseAction.Disable();

	private void OnGameStart() {
		CloseUi();
	}

	private void OnGameOver() {
		OpenUi(titleUi);
	}

	public void CloseUi() {
		if (activeUi == pauseUi) {
			GameManager.Instance.UnPauseGame();
		}
		activeUi.SetActive(false);

		if (uiStack.Count > 0) {
			activeUi = uiStack.Pop();
			activeUi.SetActive(true);
		}
	}

	public void OnPausePerformed() {
		if (GameManager.Instance.IsGameOver || GameManager.Instance.IsGamePaused) {
			return;
		}
		GameManager.Instance.PauseGame();
		OpenUi(pauseUi);
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
