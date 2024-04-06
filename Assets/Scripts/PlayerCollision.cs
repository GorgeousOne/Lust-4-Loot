using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {
	
	public GameObject heartPrefab;
	public Vector2 heartsPos;
	public float spacing = 1;

	public int maxHealth = 5;
	private int currentHealth;
	private List<GameObject> hearts = new(); // List to store heart GameObjects

	public float stackOffset = .5f;
	public float stackDist = .2f;
	private List<GameObject> hoardedItems = new();
	
	private void OnEnable() {
		currentHealth = maxHealth;
		UpdateHearts();
	}

	private void Update() {
		for (int i = 0; i < hoardedItems.Count; i++) {
			hoardedItems[i].transform.position = transform.position + Vector3.up * (i * stackDist + stackOffset);
		}
	}

	private void TakeDamage() {
		currentHealth -= 1;
		UpdateHearts();

		if (currentHealth == 0) {
		}
	}

	private void UpdateHearts() {
		// Destroy existing heart GameObjects
		foreach (GameObject heart in hearts) {
			Destroy(heart);
		}
		hearts.Clear();

		for (int i = 0; i < currentHealth; i++) {
			GameObject newHeart = Instantiate(heartPrefab, heartsPos + Vector2.right * spacing * i, Quaternion.identity, null);
			hearts.Add(newHeart);
		}
	}
	
	private void PickupItem(GameObject item) {
		Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
		rb.velocity = Vector2.zero;
		item.transform.parent = transform;
		item.layer = LayerMask.NameToLayer("Hoarded");
		ItemLogic itemLogic = item.GetComponent<ItemLogic>();
		itemLogic.onCannonBallHit.AddListener(OnItemHit);
		
		hoardedItems.Add(item);
	}

	private void OnItemHit(GameObject item) {
		int index = hoardedItems.IndexOf(item.gameObject);
		
		if (index == -1) {
			return;
		}
		for (int i = index; i < hoardedItems.Count; i++) {
			hoardedItems[i].GetComponent<ItemLogic>().Drop();
		}
		hoardedItems.RemoveRange(index, hoardedItems.Count - index);
	}
	
	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("CannonBall")) {
			TakeDamage();
		} else if (collision.gameObject.CompareTag("Collectable")) {
			PickupItem(collision.gameObject);
		}
	}

}