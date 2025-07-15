using UnityEngine;

public class SeaCreator : MonoBehaviour {

	public GameObject wavePrefab;
	public float minSpacing = .75f;
	public float maxSpacing = 1.25f;
	public float minWaveSpeed = 0.25f;
	public float maxWaveSpeed = 3;
	public float height = 12;
	
	private void Awake() {
		float offset = 0;
		int i = 0;
		
		while (offset < height) {
			GameObject newWave = Instantiate(wavePrefab, transform.position + Vector3.down * offset, Quaternion.identity, transform);
			WaveMover waveMover = newWave.GetComponent<WaveMover>();

			waveMover.speed = Random.Range(minWaveSpeed, maxWaveSpeed) * (i % 2 == 0 ? 1 : -1);
			offset += Random.Range(minSpacing, maxSpacing);
			++i;
		}
	}
}
	