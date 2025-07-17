using System.Collections;
using UnityEngine;


public enum SoundType {
	THEME,
	ARGH,
	SHOOT,
	TAKE_DAMAGE,
	PICKUP,
	COINS,
	SPLASH,
	FANFARE
}

public class SoundManager : MonoBehaviour {

	private static SoundManager instance;

	[SerializeField] private AudioSource musicSounds;
	[SerializeField] private AudioSource sfxSounds;
	[SerializeField] private AudioClip[] soundList;

	private static float sfxVolume = 1f;
	public static float SfxVolume {
		get => sfxVolume;
		set {
			sfxVolume = Mathf.Clamp(value, 0f, 1f);
			instance.TestSfx();
		}
	}

	private Coroutine delay;
	private void TestSfx() {
			if (delay != null) {
				StopCoroutine(delay);
			}
			delay = StartCoroutine(PlayDelayedTestSound());
	}

	private IEnumerator PlayDelayedTestSound() {
		yield return new WaitForSeconds(0.2f);
		PlaySfx(SoundType.ARGH);
		delay = null;
	}

	public static float MusicVolume {
		get => instance.musicSounds.volume;
		set => instance.musicSounds.volume = value;
	}

	void Awake() {
		instance = this;
	}

	public static void PlaySfx(SoundType sound, float volume = 1f, float pitch = 1f) {
		instance.sfxSounds.pitch = pitch;
		instance.sfxSounds.PlayOneShot(instance.soundList[(int)sound], volume * sfxVolume);
	}

	public static void PlayMusic(SoundType sound, float volume = 1f) {
		instance.musicSounds.clip = instance.soundList[(int)sound];
		instance.musicSounds.loop = true;
		instance.musicSounds.volume = volume;
		instance.musicSounds.Play();
	}
}
