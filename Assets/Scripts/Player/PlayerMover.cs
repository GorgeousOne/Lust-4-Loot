using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour {

	[SerializeField] private int playerIndex;
	[SerializeField] private float maxSpeed = 5f;
	[SerializeField] private float minSpeed = 1f;

	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private float bulletSpeed = 5f;
	[SerializeField] private Vector2 bulletOffset;

	[SerializeField] private float reloadTime = 0.5f;

	public int GetPlayerIndex() {
		return playerIndex;
	}

	private float currentSpeed = 5f;

	private Vector2 inputVel;
	private Rigidbody2D rigid;

	private float lastShootTime;

    private void Awake() {
		rigid = GetComponent<Rigidbody2D>();
		GetComponent<PlayerCollision>().onItemsChanged.AddListener(OnItemsChanged);
    }

    void Start() {
		GameManager.Instance.OnGameOver.AddListener(() => inputVel = Vector2.zero);
    }

    //helper function for shared keyboard input from outside
	public void SetMoveInputVec(Vector2 input) {
		if (GameManager.Instance.IsGameOver || GameManager.Instance.IsGamePaused) {
			return;
		}
		inputVel = input;
	}

	public void TriggerShootInput() {
		if (GameManager.Instance.IsGameOver || GameManager.Instance.IsGamePaused) {
			return;
		}
		if (Time.time < lastShootTime + reloadTime) {
			return;
		}
		lastShootTime = Time.time;
		FireBullet();
	}

	public GameObject FireBullet() {
		int playerFacing = playerIndex == 1 ? 1 : -1;
		Vector3 offset = bulletOffset;
		offset.x *= playerFacing;
		GameObject bullet = Instantiate(bulletPrefab, transform.position + offset, Quaternion.identity);
		bullet.layer = LayerMask.NameToLayer("Bullet" + playerIndex);

		Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
		bulletRigid.linearVelocity = Vector2.right * bulletSpeed * playerFacing;
		SoundManager.PlaySfx(SoundType.SHOOT);
		return bullet;
	}

	private void FixedUpdate() {
		rigid.linearVelocity = inputVel * currentSpeed;
	}

	private void OnItemsChanged(int count) {
		currentSpeed = minSpeed + (maxSpeed - minSpeed) * Mathf.Pow(0.66f, count);
	}
}