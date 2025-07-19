using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiAutoFocus : MonoBehaviour {

	void OnEnable() {
		StartCoroutine(FocusFirstButton());
	}

	IEnumerator FocusFirstButton() {
		yield return null;
		Button firstButton = GetComponentInChildren<Button>(true);
		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
	}
}
