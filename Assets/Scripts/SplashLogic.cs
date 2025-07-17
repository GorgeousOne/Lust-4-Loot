using System.Collections;
using UnityEngine;

public class SplashLogic : MonoBehaviour {

    void Start() {
        SoundManager.PlaySfx(SoundType.SPLASH);

        Animator anim = GetComponent<Animator>();
        AnimatorClipInfo info = anim.GetCurrentAnimatorClipInfo(0)[0];
        Destroy(gameObject, info.clip.length);
    }
}
