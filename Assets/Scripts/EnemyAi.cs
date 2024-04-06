using System;
using UnityEngine;

public class EnemyAi : MonoBehaviour {
	
	public GameObject bulletPrefab;
	public float shootInterval = 2f;
	public float bulletSpeed = 3f;
	
	public float travelTime = 3f;
	public float travelDist = 3f;
	
	private float shootTimer;
	private float spawnX;
	
	private void OnEnable() {
		shootTimer = -travelTime;
		spawnX = transform.position.x;
	}

	void Update() {
		shootTimer += Time.deltaTime;
		
		//travel when spawning instead of shooting
		if (shootTimer < 0) {
			//idk movement is somehow backwards
			float travelProgress = shootTimer / travelTime;
			float smooth = 1 - Mathf.Pow(1 - travelProgress, 2);
			Vector2 newPos = transform.position;
			newPos.x = spawnX - travelDist * smooth;
			transform.position = newPos;
		}
		else {
			if (shootTimer >= shootInterval) {
				shootTimer -= shootInterval;
				Shoot();
			}
		}
	}

	void Shoot() {
		GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
		Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
		rb.velocity = Vector2.left * bulletSpeed;
	}
}