using System.Collections;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour {

    public float maxVolume;
    private AudioSource audioSource;
    private static BackgroundMusic instance;

    private void Start() {
        instance = this;
        if (DataStorage.Music) {
            audioSource = GetComponent<AudioSource>();
            audioSource.volume = 0;
            audioSource.Play();
            StartCoroutine(FadeOut());
        }
    }

    public static BackgroundMusic GetInstance() {
        return instance;
    }

    private IEnumerator FadeOut() {
        float time = 0;
        while (audioSource.volume < maxVolume) {
            time += 0.2f;
            audioSource.volume = Mathf.Lerp(0, maxVolume, time);
            yield return null;
        }
    }

    private IEnumerator FadeIn() {
        float time = 0;
        float currentVolume = audioSource.volume;
        while (audioSource.volume > 0) {
            time += 0.2f;
            audioSource.volume = Mathf.Lerp(currentVolume, 0, time);
            yield return null;
        }
    }

    public void StartTransition() {
        if (DataStorage.Music) {
            StartCoroutine(FadeIn());
        }
    }

    public void StopMusic() {
        audioSource.Stop();
    }

    public void PlayMusic() {
        audioSource.volume = maxVolume;
        audioSource.Play();
    }

    public void MuffleMusic() {
        audioSource.volume = maxVolume / 4;
    }

    public void ContinueMusicAfterPause() {
        audioSource.volume = maxVolume;
    }
}
