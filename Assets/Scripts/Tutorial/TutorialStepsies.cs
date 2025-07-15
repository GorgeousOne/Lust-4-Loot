using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialStepsies : MonoBehaviour {

	public TutorialRunner runner;

	public GameObject player1;
	public GameObject player2;
	public GameObject dashedBoxesContainer;
	public GameObject lootContainer;
	public GameObject IngameCanvas;
	public Slider tugOfWarMeter;
	public IslandLogic island;


	public void OnEnable() {
		var steps = new List<TutorialStep>() {
			new TutorialStep {
				message = "Each pirate gets thar own territory!",
				onStepEnter = () => ShowTerritory(),
				onStepExit = () => HideTerritory(),
			},
			new TutorialStep {
				message = "Sail the seas 'n hoard yer booty!",
				onStepEnter = () => AnimateHoarding(),
				onStepExit = () => StopHoarding()
			},
			new TutorialStep {
				message = "Return t' Treasure Island t' cash in yer doubloons!",
				onStepEnter = () => CashIn(),
				onStepExit = () => CashOut(),
			},
			new TutorialStep {
				message = "Blast yer rival wit' cannonballs!",
				onStepEnter = () => StartShooting(),
				onStepExit = () => StopShooting(),
			},
			new TutorialStep {
				message = "First scallywag t' fill their meter wins!",
				onStepEnter = () => MoveMeter(),
				onStepExit = () => StopMovingMeter()
			}
		};
		runner.Init(steps);
	}

	void ShowTerritory() {
		island.transform.position = Vector2.zero;
		island.gameObject.SetActive(true);
		dashedBoxesContainer.SetActive(true);
		Debug.Log("step 1");
	}

	void HideTerritory() {
		dashedBoxesContainer.SetActive(false);
	}

	void AnimateHoarding() {
		lootContainer.SetActive(true);
		Debug.Log("step 2");
	}

	void StopHoarding() {
	}

	void CashIn() {

	}

	void CashOut() {

	}

	void StartShooting() {

	}

	void StopShooting() {

	}

	void MoveMeter() {

	}

	void StopMovingMeter() {
		tugOfWarMeter.value = 0.5f;
	}
}
