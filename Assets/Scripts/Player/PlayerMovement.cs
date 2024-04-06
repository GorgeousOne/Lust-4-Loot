using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

	public int playerNumber = 1;
	public float maxSpeed = 5f;
	public float minSpeed = 1f;
	
	private float currentSpeed = 5f;
	
	
	public GameObject bulletPrefab;
	public float bulletSpeed = 5f;
	public float reloadTime = 0.5f;
	
	private Vector2 inputVel;
	private Rigidbody2D rb;
	
	private float lastShootTime;
	
	private void OnEnable() {
		rb = GetComponent<Rigidbody2D>();
		GetComponent<PlayerCollision>().onItemsChanged.AddListener(OnItemsChanged);
	}

	public void OnShoot(){
		if (Time.time < lastShootTime + reloadTime) {
			return;
		}
		lastShootTime = Time.time;
		int playerFacing = playerNumber == 1 ? 1 : -1;
		GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

		bullet.layer = LayerMask.NameToLayer("Bullet" + playerNumber);
		
		Rigidbody2D billetRb = bullet.GetComponent<Rigidbody2D>();
		billetRb.velocity = Vector2.right * bulletSpeed * playerFacing;
	}

	public void OnMove( InputAction.CallbackContext context ) {
		inputVel = context.ReadValue<Vector2>();
	}

	private void FixedUpdate() {
		rb.velocity = inputVel * currentSpeed;
	}
	
	private void OnItemsChanged(int count) {
		currentSpeed = minSpeed + (maxSpeed - minSpeed) * Mathf.Pow(0.66f, count);
	}
}