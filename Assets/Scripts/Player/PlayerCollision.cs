using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCollision : MonoBehaviour {

	public int playerNumber = 1;
	public Vector2 stackOffset = Vector2.zero;
	public float stackDist = .2f;
	public UnityEvent<int> onItemsChanged;
	
	private List<GameObject> hoardedItems = new();
	
	private void Update() {
		for (int i = 0; i < hoardedItems.Count; i++) {
			Vector3 itemPos = (Vector3) stackOffset + i * stackDist * Vector3.up;
			
			if (playerNumber == 2) {
				itemPos.x *= -1;
			}
			hoardedItems[i].transform.position = transform.position + itemPos;
		}
	}
	
	public void UnloadItems() {
		foreach (GameObject item in hoardedItems) {
			Destroy(item.GetComponent<ItemLogic>());
		}
		hoardedItems.Clear();
		onItemsChanged.Invoke(hoardedItems.Count);
	}
	
	public int getItemCount() {
		return hoardedItems.Count;
	}
	
	
	private void TakeDamage() {
		foreach (GameObject item in hoardedItems) {
			item.GetComponent<ItemLogic>().Drop();
		}
		hoardedItems.Clear();
		onItemsChanged.Invoke(hoardedItems.Count);
	}
	
	private void PickupItem(GameObject item) {
		Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
		rb.velocity = Vector2.zero;
		item.transform.parent = transform;
		item.layer = LayerMask.NameToLayer("Hoarded" + playerNumber);
		ItemLogic itemLogic = item.GetComponent<ItemLogic>();
		itemLogic.onCannonBallHit.AddListener(OnItemHit);
		
		hoardedItems.Add(item);
		onItemsChanged.Invoke(hoardedItems.Count);
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
		onItemsChanged.Invoke(hoardedItems.Count);
	}
	
	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("CannonBall")) {
			TakeDamage();
		} else if (collision.gameObject.CompareTag("Collectable")) {
			PickupItem(collision.gameObject);
		}
	}
}