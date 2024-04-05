using UnityEngine;

public class BallLogic : MonoBehaviour {
	private void OnCollisionEnter2D(Collision2D collision) {
		Destroy(gameObject, 0.1f);
	}
}