using UnityEngine;
using UnityEngine.UI;

public class TugAnimator : MonoBehaviour {

	[SerializeField] float snapTime = 0.5f;

	private Slider slider;
	private float startTime;
	private float startValue;
	private float targetValue = 0.5f;


	void Awake() {
		slider = GetComponent<Slider>();
	}

	void Update() {
		float dt = Time.time - startTime;
		float t = dt / snapTime;

		if (t < 1) {
			slider.value = Mathf.SmoothStep(startValue, targetValue, t);
		} else {
			slider.value = targetValue;
		}
	}

	public void SetMeter(float percent) {
		startTime = Time.time;
		startValue = slider.value;
		targetValue = Mathf.Clamp01(percent);
	}
}
