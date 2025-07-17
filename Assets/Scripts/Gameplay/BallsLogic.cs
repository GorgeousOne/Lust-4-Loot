using UnityEngine;
using UnityEngine.Serialization;

public class BallsLogic : Droppable {

    private new void Drop() {
		base.Drop();
		Destroy(GetComponent<Collider2D>());
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		Drop();
	}
}