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

    public void Init(List<TutorialStep> steps) {
        nextButton.onClick.AddListener(AdvanceStep);
        this.steps = steps;
        currentStep = 0;
        ShowStep(currentStep);
    }

    void AdvanceStep() {
        ++currentStep;
        ShowStep(currentStep);
        Debug.Log("step" + currentStep);
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

    void EndTutorial() {
        UiManager.Instance.CloseUi();
        GameManager.Instance.StartGame();
        gameObject.SetActive(false);
    }
}
