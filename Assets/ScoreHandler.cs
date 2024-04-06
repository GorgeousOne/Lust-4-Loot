using UnityEngine;
using UnityEngine.UI;
public class ScoreHandler : MonoBehaviour {

	//ui slider to display the score
	public Slider scoreSlider;
	
	public int scoreRange = 50;
	public int currentScore;
	
	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag("Player1")) {
			PlayerCollision player = other.gameObject.GetComponent<PlayerCollision>();
			int itemCount = player.getItemCount();
			currentScore += itemCount;
			player.UnloadItems();
		} else if (other.gameObject.CompareTag("Player2")) {
			PlayerCollision player = other.gameObject.GetComponent<PlayerCollision>();
			int itemCount = player.getItemCount();
			currentScore -= itemCount;
			player.UnloadItems();
		}
		else {
			return;
		}
		scoreSlider.value = Remap(currentScore, -scoreRange, scoreRange, 0, 1);
		// Debug.Log("balance" + currentScore + " " + scoreSlider.value);
		
		if (currentScore >= scoreRange) {
			Debug.Log("Player 1 wins!");
		} else if (currentScore <= -scoreRange) {
			Debug.Log("Player 2 wins!");
		}
	}
	
	private static float Remap(float x, float min, float max, float min2, float max2) {
		return (x - min) / (max - min) * (max2 - min2) + min2;
	}
}