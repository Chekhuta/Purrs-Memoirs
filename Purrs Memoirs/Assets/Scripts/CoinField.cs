using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoinField : MonoBehaviour {

    public RectTransform background;
    public Text coinValueText;
    public RectTransform coinRect;
    public Text bonusTexts;
    private bool isBonusTextUsing = false;

    private void Start() {
        UpdateCoinField();
    }

    public RectTransform GetCoinRectParent() {
        return GetComponent<RectTransform>();
    }

    public Vector3 GetCoinImagePosition() {
        return coinRect.localPosition;
    }

    public void UpdateCoinField() {
        int coins = TasksData.Coins;
        int countOfDigits = 1;

        while ((coins /= 10) != 0) {
            countOfDigits++;
        }

        background.anchoredPosition = new Vector2(20 - (6 * (countOfDigits - 1)), background.anchoredPosition.y);
        background.sizeDelta = new Vector2(44 + (12 * (countOfDigits - 1)), background.sizeDelta.y);

        coinValueText.text = TasksData.Coins + "";
    }

    public void ShowBonusText(int reward) {
        StartCoroutine(HideBonusText(reward));
    }

    private IEnumerator HideBonusText(int reward) {
        while (isBonusTextUsing) {
            yield return new WaitForSeconds(0.1f);
        }
        isBonusTextUsing = true;

        bonusTexts.text = "+" + reward + "";
        bonusTexts.gameObject.SetActive(true);

        RectTransform textRect = bonusTexts.GetComponent <RectTransform>();
        float startPositionY = textRect.localPosition.y;
        float finalPositionY = startPositionY + 10;
        float movingTime = 0;
        Color textColor = bonusTexts.color;

        yield return new WaitForSeconds(1f);

        while (textRect.localPosition.y != finalPositionY) {
            movingTime += 0.2f;
            textColor.a = Mathf.Lerp(1, 0, movingTime);
            bonusTexts.color = textColor;
            float _interpolatedScaleValueY = Mathf.Lerp(textRect.localPosition.y, finalPositionY, movingTime);
            textRect.localPosition = new Vector3(textRect.localPosition.x, _interpolatedScaleValueY, textRect.localPosition.z);
            yield return new WaitForFixedUpdate();
        }
        textRect.localPosition = new Vector3(textRect.localPosition.x, startPositionY, textRect.localPosition.z);
        textRect.gameObject.SetActive(false);
        textColor.a = 1;
        bonusTexts.color = textColor;
        isBonusTextUsing = false;
    }
}
