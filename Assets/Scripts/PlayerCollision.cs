using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {

	public int playerNumber = 1;

	public float stackOffset = .5f;
	public float stackDist = .2f;
	private List<GameObject> hoardedItems = new();
	
	private void OnEnable() {
	}

	private void Update() {
		for (int i = 0; i < hoardedItems.Count; i++) {
			hoardedItems[i].transform.position = transform.position + Vector3.up * (i * stackDist + stackOffset);
		}
	}

	private void TakeDamage() {
	}
	
	private void PickupItem(GameObject item) {
		Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
		rb.velocity = Vector2.zero;
		item.transform.parent = transform;
		item.layer = LayerMask.NameToLayer("Hoarded" + playerNumber);
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