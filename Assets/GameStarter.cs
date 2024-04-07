using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour {
	
	public Canvas menuCanvas;
	public GameObject scoreSlider;
	public GameObject island;
	
	public void OnButtonPress() {
		//hide the menu on start button press
		menuCanvas.gameObject.SetActive(false);
		scoreSlider.SetActive(true);
		island.SetActive(true);
		
		//find all PlayerMovement scripts and enable them
		PlayerMovement[] players = FindObjectsOfType<PlayerMovement>();
		foreach (PlayerMovement player in players) {
			player.enabled = true;
		}
	}
}