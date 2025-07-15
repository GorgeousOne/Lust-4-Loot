using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialStepsies : MonoBehaviour {

	public TutorialRunner runner;

	public GameObject player1;
	public GameObject player2;
	public GameObject dashedBoxesContainer;
	public GameObject lootContainer;
	public GameObject ingameUi;
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
		island.transform.position = new Vector2(0, -2);
		island.gameObject.SetActive(true);
		dashedBoxesContainer.SetActive(true);
		ingameUi.SetActive(true);
	}

	void HideTerritory() {
		dashedBoxesContainer.SetActive(false);
	}

	bool circleAnimEnded = false;
	bool cashAnimEnded = false;
	bool meterAnimEnded = false;
	GameObject animBullet;

	void AnimateHoarding() {
		lootContainer.SetActive(true);
		StartCoroutine(CirclePlayers());
	}

	IEnumerator CirclePlayers() {
		float elapsed = 0f;
		float duration = 2f;

		var center1 = new Vector3(-5, 0);
		var center2 = new Vector3(5, 0);
		float radius = 2;
				
		while (elapsed < duration && !circleAnimEnded) {
			elapsed += Time.deltaTime;
			float angle = Mathf.SmoothStep(0, 360, elapsed / duration);

			player1.transform.position = center1 + Quaternion.Euler(0, 0, 180 + angle) * Vector2.right * radius;
			player2.transform.position = center2 + Quaternion.Euler(0, 0, angle) * Vector2.right * radius;
			yield return null;
		}
	}

	void StopHoarding() {
		circleAnimEnded = true;
		lootContainer.SetActive(false);
		var playerPickup1 = player1.GetComponent<PlayerCollision>();
		var playerPickup2 = player2.GetComponent<PlayerCollision>();

		//pickup any skipped items?
		foreach (Transform child in lootContainer.transform) {
			if (!child.parent.tag.Contains("Player")) {
				var pos = transform.position;
				if ((player1.transform.position - pos).sqrMagnitude <
					(player2.transform.position - pos).sqrMagnitude) {
					playerPickup1.PickupItem(child.gameObject);
				} else {
					playerPickup2.PickupItem(child.gameObject);
				}
			}
		}
		player1.transform.position = new Vector2(-7, 0);
		player2.transform.position = new Vector2(7, 0);
	}

	void CashIn() {
		StartCoroutine(MoveP1());
	}

	IEnumerator MoveP1() {
		float elapsed = 0f;
		float duration = 1f;

		var start = new Vector2(-7, 0);
		var end = new Vector2(-1, -1);

		while (elapsed < duration && !cashAnimEnded) {
			float t = Mathf.SmoothStep(0, 1, elapsed / duration);
			player1.transform.position = Vector3.Lerp(start, end, t);
			yield return null;
			elapsed += Time.deltaTime;
		}
		player1.transform.position = end;
		yield return new WaitForSeconds(1);
		elapsed = 0;

		while (elapsed < duration && !cashAnimEnded) {
			float t = Mathf.SmoothStep(0, 1, elapsed / duration);
			player1.transform.position = Vector3.Lerp(end, start, t);
			yield return null;
			elapsed += Time.deltaTime;
		}
	}

	void CashOut() {
		cashAnimEnded = true;
		player1.GetComponent<PlayerCollision>().TakeDamage();
		player1.transform.position = new Vector2(-7, 0);
		player2.transform.position = new Vector2(7, 0);
	}

	void StartShooting() {
		player2.transform.position = new Vector2(7, 0.5f);
		// animBullet = player1.GetComponent<PlayerMove>().FireBullet();
		// print("i SHOT " + animBullet.tag);
	}

	void StopShooting() {
		player2.transform.position = new Vector2(7, 0f);
		if (animBullet) {
			Destroy(animBullet);
		}
	}

	void MoveMeter() {
		StartCoroutine(diddleMeter());
	}

	IEnumerator diddleMeter() {
		float[] jumps = { 0.3f, 0.8f, 0.2f, 0.5f };

		foreach(float val in jumps) {
			if (meterAnimEnded) {
				break;
			}
			tugOfWarMeter.value = val;
			yield return new WaitForSeconds(0.6f);
		}
	}

	void StopMovingMeter() {
		meterAnimEnded = true;
		tugOfWarMeter.value = 0.5f;
	}
}
