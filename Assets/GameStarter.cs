using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour {
	
	public Canvas menuCanvas;
	public GameObject scoreSlider;
	public GameObject island;
	public GameObject itemSpawner;
	public GameObject player1;
	public GameObject player2;
	
	public void OnButtonPress() {
		//hide the menu on start button press
		menuCanvas.gameObject.SetActive(false);
		scoreSlider.SetActive(true);
		island.SetActive(true);
		itemSpawner.SetActive(true);
		
		player1.transform.position = new Vector3(-7, 0, 0);
		player2.transform.position = new Vector3(7, 0, 0);
		
		//find all PlayerMovement scripts and enable them
		PlayerMovement[] players = FindObjectsOfType<PlayerMovement>();
		foreach (PlayerMovement player in players) {
			player.enabled = true;
		}
	}
}