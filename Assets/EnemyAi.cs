using UnityEngine;

public class EnemyAi : MonoBehaviour {
	
	private float shootTimer;
	public float shootInterval = 2f;
	public float bulletSpeed = 3f;
	public GameObject bulletPrefab;
	
	void Update() {
		shootTimer += Time.deltaTime;

		if (shootTimer >= shootInterval) {
			shootTimer -= shootInterval;
			Shoot();
		}
	}

	void Shoot() {
		GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
		Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
		rb.velocity = Vector2.left * bulletSpeed;
		Debug.Log("BOOOM");
	}
}