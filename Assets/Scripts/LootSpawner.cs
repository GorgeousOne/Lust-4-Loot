using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{

    public GameObject collectablePrefab;
    public float spawnInterval = 2f;
    public float spawnSpeed = 3f;
    public float spawnRange = 8f;
    public float spawnDistX = 10f;
    
    private float spawnTimer;

    private void OnEnable() {
    }

    void Update() {
        spawnTimer += Time.deltaTime;
        
        if (spawnTimer >= spawnInterval) {
            spawnTimer -= spawnInterval;
            SpawnLoot();
        }
    }
    
    void SpawnLoot() {
        Vector3 spawnOff = Vector2.up * Random.Range(-spawnRange, spawnRange) + Vector2.left * spawnDistX;
        GameObject loot = Instantiate(collectablePrefab, transform.position + spawnOff, Quaternion.identity);

        Vector3 spawnOff2 = Vector2.up * Random.Range(-spawnRange, spawnRange) + Vector2.right * spawnDistX;
        GameObject loot2 = Instantiate(collectablePrefab, transform.position - spawnOff, Quaternion.identity);
        loot2.GetComponent<ItemLogic>().speed *= -1;
    }
}
