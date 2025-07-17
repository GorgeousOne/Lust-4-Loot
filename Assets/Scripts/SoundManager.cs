using System.Collections;
using System.Collections.Generic;
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
	[SerializeField] private AudioClip[] soundList;
	[SerializeField] private int initialSfxPoolSize = 10;

	private static float sfxVolume = 1f;

	private List<AudioSource> sfxPool = new List<AudioSource>();
	private Transform sfxParent;

	private Coroutine delay;

	public static float SfxVolume {
		get => sfxVolume;
		set {
			sfxVolume = Mathf.Clamp(value, 0f, 1f);
			instance.TestSfx();
		}
	}

	public static float MusicVolume {
		get => instance.musicSounds.volume;
		set => instance.musicSounds.volume = value;
	}

	void Awake() {
		instance = this;
		sfxParent = new GameObject("SFXPool").transform;
		sfxParent.parent = transform;

		for (int i = 0; i < initialSfxPoolSize; i++) {
			sfxPool.Add(CreateNewSfxSource());
		}
	}

	private AudioSource CreateNewSfxSource() {
		var source = gameObject.AddComponent<AudioSource>();
		source.playOnAwake = false;
		source.loop = false;
		source.spatialBlend = 0f; // 2D sound
		source.transform.parent = sfxParent;
		return source;
	}

	private AudioSource GetAvailableSource() {
		foreach (var src in sfxPool) {
			if (!src.isPlaying) return src;
		}
		// Pool exhausted, create a new one on the fly
		var newSource = CreateNewSfxSource();
		sfxPool.Add(newSource);
		return newSource;
	}

	private void TestSfx() {
		if (delay != null) StopCoroutine(delay);
		delay = StartCoroutine(PlayDelayedTestSound());
	}

	private IEnumerator PlayDelayedTestSound() {
		yield return new WaitForSeconds(0.2f);
		PlaySfx(SoundType.ARGH);
		delay = null;
	}

	public static void PlaySfx(SoundType sound, float volume = 1f, float pitch = 1f) {
		var src = instance.GetAvailableSource();
		float deflangedPitch = Random.Range(pitch * 0.98f, pitch * 1.02f);

		src.clip = instance.soundList[(int)sound];
		src.volume = volume * sfxVolume;
		src.pitch = deflangedPitch;
		src.Play();
	}

	public static void PlayMusic(SoundType sound, float volume = 1f) {
		instance.musicSounds.clip = instance.soundList[(int)sound];
		instance.musicSounds.loop = true;
		instance.musicSounds.volume = volume;
		instance.musicSounds.Play();
	}
}
