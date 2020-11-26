using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RatePopup : MonoBehaviour {

    public static bool WasPopupShowed { get; set; }
    public static int GamePlayedDelay { get; set; } = 0;
    public Text rateTitleText;
    public Text rateMainText;
    public Text laterText;
    public Text rateText;
    public Image fadeBackground;
    public RectTransform popupRect;
    private bool buttonActive = true;

    public void RateGame() {
        if (!buttonActive) {
            return;
        }
        buttonActive = false;
        DataStorage.AddRateGameShow(3);
        Application.OpenURL("");
        StartCoroutine(CloseAnimation());
    }

    public void ShowPopup() {
        LanguageTitles languageTitles = LanguageTitles.GetInstance();
        gameObject.SetActive(true);
        WasPopupShowed = true;
        rateTitleText.text = languageTitles.rateTitle;
        rateMainText.text = languageTitles.rateMainTitle;
        rateText.text = languageTitles.rateButtonTitle;
        if (DataStorage.LanguageId == 0) {
            rateText.fontSize = 21;
        }
        laterText.text = languageTitles.laterButtonTitle;
        popupRect.localScale = new Vector3(2, 0, 0);
        StartCoroutine(ShowAnimation());
    }

    public void RateLater() {
        if (!buttonActive) {
            return;
        }
        buttonActive = false;
        DataStorage.AddRateGameShow(1);
        ClosePopup();
    }

    public void ClosePopup() {
        StartCoroutine(CloseAnimation());
    }

    private IEnumerator ShowAnimation() {

        yield return new WaitForSeconds(0.2f);

        float movingTime = 0;
        Color fadeColor = fadeBackground.color;

        while (popupRect.localScale.y != 2) {
            movingTime += 0.08f;
            float _interpolatedScaleValue = Mathf.Lerp(0, 2, movingTime);
            fadeColor.a = Mathf.Lerp(0, 1, movingTime);
            popupRect.localScale = new Vector3(_interpolatedScaleValue, _interpolatedScaleValue, 0);
            fadeBackground.color = fadeColor;
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator CloseAnimation() {

        float movingTime = 0;
        Color fadeColor = fadeBackground.color;

        while (popupRect.localScale.y != 0) {
            movingTime += 0.08f;
            float _interpolatedScaleValue = Mathf.Lerp(2, 0, movingTime);
            fadeColor.a = Mathf.Lerp(1, 0, movingTime); ;
            popupRect.localScale = new Vector3(_interpolatedScaleValue, _interpolatedScaleValue, 0);
            fadeBackground.color = fadeColor;
            yield return new WaitForFixedUpdate();
        }
        FindObjectOfType<Game>().ShowLevelCompletePanel();
        gameObject.SetActive(false);
    }

}
