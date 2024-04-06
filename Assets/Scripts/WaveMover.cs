using System;
using UnityEngine;

public class WaveMover : MonoBehaviour {

	public float speed = 1;
	public float waveLength = 4;

	private float startX;
	
	private void OnEnable() {
		startX = transform.position.x;
	}

	void Update() {
		float movement = Mathf.Repeat(Time.time * speed, waveLength);
		transform.position = new Vector2(startX + movement, transform.position.y);
	}
}