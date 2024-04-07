using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour {
	
	public Canvas menuCanvas;

	public void OnButtonPress() {
		//hide the menu on start button press
		menuCanvas.gameObject.SetActive(false);
		
		//find all PlayerMovement scripts and enable them
		PlayerMovement[] players = FindObjectsOfType<PlayerMovement>();
		foreach (PlayerMovement player in players) {
			player.enabled = true;
		}
	}
}