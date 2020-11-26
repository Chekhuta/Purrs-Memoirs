using UnityEngine;

public class Sounds : MonoBehaviour {

    public AudioClip clickSound;
    public AudioClip lightClickSound;
    public AudioClip coinSound;
    public AudioSource conveyorAudioSource;
    private AudioSource audioSource;
    private static Sounds instance;

    private void Start() {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public static Sounds GetInstance() {
        return instance;
    }

    public void PlayClickSound() {
        if (DataStorage.Sound) {
            audioSource.PlayOneShot(clickSound, 1f);
        }
    }

    public void PlayLightClickSound() {
        if (DataStorage.Sound) {
            audioSource.PlayOneShot(lightClickSound, 1f);
        }
    }

    public void PlayCoinSound() {
        if (DataStorage.Sound) {
            audioSource.PlayOneShot(coinSound, 0.6f);
        }
    }

    public void PlayConveyorSound() {
        if (DataStorage.Sound) {
            conveyorAudioSource.Play();
        }
    }

    public void StopConveyorSound() {
        if (DataStorage.Sound) {
            conveyorAudioSource.Stop();
        }
    }
}
