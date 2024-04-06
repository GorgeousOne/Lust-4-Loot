using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
	public GameObject heartPrefab;
	public Vector2 heartsPos;
	public float spacing = 1;

	public int maxHealth = 5;
	private int currentHealth;
	private List<GameObject> hearts = new(); // List to store heart GameObjects

	private void OnEnable() {
		currentHealth = maxHealth;
		UpdateHearts();
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

		hearts.Clear(); // Clear the list

		// Instantiate new heart GameObjects
		for (int i = 0; i < currentHealth; i++) {
			GameObject newHeart = Instantiate(heartPrefab, heartsPos + Vector2.right * spacing * i, Quaternion.identity, null);
			hearts.Add(newHeart); // Add the instantiated heart to the list
		}
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("CannonBall")) {
			Debug.Log("OUCHI BOUCHY");
			// Destroy(collision.gameObject);
			TakeDamage();
		}
	}
}