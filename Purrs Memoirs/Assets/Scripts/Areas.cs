using UnityEngine;
using UnityEngine.UI;

public class Areas : MonoBehaviour {

    public Text starsText;

    public AudioClip clickSound;
    private AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        starsText.text = DataStorage.GetSumOfStars() + " / 90";
    }

    public void OpenLevelsScene() {
        PlayClickSound();
        Transition.GetInstance().EndScene("Levels");
    }

    public void ReturnToMenu() {
        PlayClickSound();
        Transition.GetInstance().EndScene("Menu");
    }

    public void PlayClickSound() {
        if (DataStorage.Sound) {
            audioSource.PlayOneShot(clickSound, 1f);
        }
    }
}
