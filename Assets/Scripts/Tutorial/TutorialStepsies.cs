using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialStepsies : MonoBehaviour {

	public TutorialRunner runner;

	public GameObject player1;
	public GameObject player2;
	public GameObject dashedBoxesContainer;
	public GameObject lootPrefab;

	public GameObject ingameUi;
	public TugAnimator tugOfWarMeter;
	public IslandLogic island;

	private List<GameObject> demoLoot = new();

	private bool circleAnimEnded;
	private bool cashAnimEnded;
	private bool meterAnimEnded;
	private GameObject animBullet;

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

		island.transform.position = new Vector2(0, -2);
		island.gameObject.SetActive(true);
		ingameUi.SetActive(true);

		circleAnimEnded = false;
		cashAnimEnded = false;
		meterAnimEnded = false;

		runner.Init(steps);
	}

	void ShowTerritory() {
		player1.transform.position = new Vector2(-7, 0);
		player2.transform.position = new Vector2(7, 0);
		dashedBoxesContainer.SetActive(true);
	}

	void HideTerritory() {
		dashedBoxesContainer.SetActive(false);
	}

	void AnimateHoarding() {
		demoLoot.Add(Instantiate(lootPrefab, new Vector2(-5f, 2f), Quaternion.identity));
		demoLoot.Add(Instantiate(lootPrefab, new Vector2(-5f, -2f), Quaternion.identity));
		demoLoot.Add(Instantiate(lootPrefab, new Vector2(5f, 2f), Quaternion.identity));
		demoLoot.Add(Instantiate(lootPrefab, new Vector2(5f, -2f), Quaternion.identity));
		demoLoot[0].name = "1";
		demoLoot[1].name = "1";

		foreach (var item in demoLoot) {
			item.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
		}
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
		player1.transform.position = new Vector2(-7, 0);
		player2.transform.position = new Vector2(7, 0);
		var playerPickup1 = player1.GetComponent<PlayerCollision>();
		var playerPickup2 = player2.GetComponent<PlayerCollision>();

		//pickup any skipped items?
		foreach (var child in demoLoot) {
			if (!child.transform.parent.tag.Contains("Player")) {
				if (child.name == "1") {
					playerPickup1.PickupItem(child);
				} else {
					playerPickup2.PickupItem(child);
				}
			}
		}
		demoLoot.Clear();
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
		animBullet = player1.GetComponent<PlayerMove>().FireBullet();
	}

	void StopShooting() {
		if (animBullet) {
			Destroy(animBullet);
		}
	}

	void MoveMeter() {
		StartCoroutine(DiddleMeter());
	}

	IEnumerator DiddleMeter() {
		float[] jumps = { 0f, 1f, 0.5f };

		foreach(float val in jumps) {
			if (meterAnimEnded) {
				break;
			}
			tugOfWarMeter.SetMeter(val);
			yield return new WaitForSeconds(1f);
		}
	}

	void StopMovingMeter() {
		meterAnimEnded = true;
		tugOfWarMeter.SetMeter(0.5f);
	}
}
