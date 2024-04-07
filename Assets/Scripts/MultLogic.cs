using UnityEngine;

public class MultLogic : MonoBehaviour {
	
	public float riseTime = 0.5f;
	public float riseDist = 1f;
	public float stayTime = 0.5f;

	private float spawnTime;
	private float spawnY;
	private void OnEnable() {
		spawnTime = Time.time;
		spawnY = transform.position.y;
		Destroy(gameObject, riseTime + stayTime);
	}

	private void Update() {
		if (Time.time <= spawnTime + riseTime) {
			float riseProgress = (Time.time - spawnTime) / riseTime;
			float smooth = 1 - Mathf.Pow(1 - riseProgress, 3);
			Vector3 newPos = transform.position;
			newPos.y = Mathf.Lerp(spawnY, spawnY + riseDist, smooth);
			transform.position = newPos;
		}
	}
}
