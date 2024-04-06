using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	
	public GameObject enemyPrefab;
	public float spawnInterval = 2f;
	public float spawnSpeed = 3f;
	public float spawnRange = 4f;
	
	private float spawnTimer;
	
	private void OnEnable() {
	}
	
	void Update() {
		spawnTimer += Time.deltaTime;
		
		if (spawnTimer >= spawnInterval) {
			spawnTimer -= spawnInterval;
			SpawnEnemy();
		}
	}
	
	void SpawnEnemy() {
		Vector3 spawnOff = Vector2.up * Random.Range(-spawnRange, spawnRange);
		GameObject enemy = Instantiate(enemyPrefab, transform.position + spawnOff, Quaternion.identity);
	}
}
