using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour {

	public int playerNumber = 1;
	public float maxSpeed = 5f;
	public float minSpeed = 1f;

	private float currentSpeed = 5f;

	public GameObject bulletPrefab;
	public float bulletSpeed = 5f;
	public float reloadTime = 0.5f;

	public AudioSource fireSound;

	private Vector2 inputVel;
	private Rigidbody2D rb;

	private float lastShootTime;

	private void OnEnable() {
		rb = GetComponent<Rigidbody2D>();
		GetComponent<PlayerCollision>().onItemsChanged.AddListener(OnItemsChanged);
	}

	public void OnShoot() {
		if (GameManager.Instance.IsGameOver || GameManager.Instance.IsGamePaused) {
			return;
		}
		if (Time.time < lastShootTime + reloadTime) {
			return;
		}
		FireBullet();
	}

	public GameObject FireBullet() {
		lastShootTime = Time.time;
		int playerFacing = playerNumber == 1 ? 1 : -1;
		GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

		bullet.layer = LayerMask.NameToLayer("Bullet" + playerNumber);

		Rigidbody2D billetRb = bullet.GetComponent<Rigidbody2D>();
		billetRb.linearVelocity = Vector2.right * bulletSpeed * playerFacing;
		fireSound.Play();
		return bullet;
	}

	public void OnMove(InputAction.CallbackContext context) {
		Velocitate(context.ReadValue<Vector2>());
	}

	//helper function for shared keyboard input from outside
	public void Velocitate(Vector2 input) {
		if (GameManager.Instance.IsGameOver || GameManager.Instance.IsGamePaused) {
			return;
		}
		inputVel = input;
	}

	private void FixedUpdate() {
		rb.linearVelocity = inputVel * currentSpeed;
	}

	private void OnItemsChanged(int count) {
		currentSpeed = minSpeed + (maxSpeed - minSpeed) * Mathf.Pow(0.66f, count);
	}
}