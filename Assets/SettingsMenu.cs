using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

	[SerializeField] private Button backButton;

	private void Start() {
		backButton.onClick.AddListener(() => UiManager.Instance.CloseSettings());
	}

	public void OnMusicVolumeChange() {

	}

	public void OnSfxVolumeChange() {

	}
}
