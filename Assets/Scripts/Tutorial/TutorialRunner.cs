using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TutorialStep {
	public string message;
	public Action onStepEnter;
	public Action onStepExit;
}

public class TutorialRunner : MonoBehaviour {

	public TextMeshProUGUI tutorialText;
	public Button nextButton;

	private List<TutorialStep> steps;
	private int currentStep;

    public void Start() { 
		nextButton.onClick.AddListener(AdvanceStep);		
    }

    public void Init(List<TutorialStep> steps) {
		this.steps = steps;
		currentStep = 0;
		ShowStep(currentStep);
	}

	void ShowStep(int index) {
		if (index >= steps.Count) {
			EndTutorial();
			return;
		}
		if (index > 0) {
			steps[index - 1].onStepExit?.Invoke();
		}
		var step = steps[index];
		tutorialText.text = step.message;
		step.onStepEnter?.Invoke();
	}

	void AdvanceStep() {
		++currentStep;
		ShowStep(currentStep);
	}

	void EndTutorial() {
		UiManager.Instance.CloseUi();
		GameManager.Instance.StartGame();
		gameObject.SetActive(false);
	}
}
