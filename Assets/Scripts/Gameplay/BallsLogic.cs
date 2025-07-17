using UnityEngine;
using UnityEngine.Serialization;

public class BallsLogic : Droppable {

    private new void Drop() {
		base.Drop();
		Destroy(GetComponent<Collider2D>());
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		//remove item on contact with game bounds
		if (collision.gameObject.layer == LayerMask.NameToLayer("Default")) {
			Destroy(gameObject);
		} else {
			Drop();
		}
	}
}