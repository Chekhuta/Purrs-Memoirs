using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMessage : MonoBehaviour {

    public GameObject purrImage;
    public GameObject whiskerImage;
    public Text messageText;
    public Button confirmButton;
    public Button rejectButton;
    public AudioClip clickSound;
    private Tutorial tutorial;
    private RectTransform windowRect;
    private float windowHeight = 135.6f;
    private AudioSource audioSource;

    public void SetTutorial(Tutorial t) {
        audioSource = GetComponent<AudioSource>();
        tutorial = t;
        windowRect = GetComponent<RectTransform>();
    }

    public void SetMessage(string message) {
        messageText.text = message;
        StartCoroutine(ChangeWindowHeight());
    }

    private IEnumerator ChangeWindowHeight() {
        yield return null;
        windowRect.sizeDelta = new Vector2(windowRect.sizeDelta.x, (int)(messageText.GetComponent<RectTransform>().sizeDelta.y + windowHeight));
    }

    public void ConfirmButtonAction() {
        PlayClickSound();
        tutorial.ConfirmButtonPressed();
    }

    public void RejectButtonAction() {
        PlayClickSound();
        tutorial.RejectButtonPressed();
    }

    public void OneButtonPattern() {
        windowHeight = 91.6f;
        rejectButton.gameObject.SetActive(false);
    }

    public void SetConfirmButtonText(string buttonText) {
        confirmButton.GetComponentInChildren<Text>().text = buttonText;
    }

    public void SetRejectButtonText(string buttonText) {
        rejectButton.GetComponentInChildren<Text>().text = buttonText;
    }

    public void ShowPurrImage() {
        purrImage.SetActive(true);
        whiskerImage.SetActive(false);
    }

    public void ShowWhiskerImage() {
        purrImage.SetActive(false);
        whiskerImage.SetActive(true);
    }

    private void PlayClickSound() {
        if (DataStorage.Sound) {
            audioSource.PlayOneShot(clickSound, 1f);
        }
    }
}
