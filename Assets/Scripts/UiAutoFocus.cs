using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiAutoFocus : MonoBehaviour {

	void OnEnable() {
		Button firstButton = GetComponentInChildren<Button>(true);
		EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
	}
}
