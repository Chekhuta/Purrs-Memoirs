using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UILevelBar : MonoBehaviour {

    public Text nameText1;
    public Text valueText1;
    public Text nameText2;
    public Text valueText2;
    public Text bonusTimeText;
    public GameObject lockImage;
    public GameObject arrowTurnPlayer;
    public GameObject arrowTurnOpponent; 
    private static UILevelBar instance;

    private void Awake() {
        instance = this;
    }

    public static UILevelBar GetInstance() {
        return instance;
    }

    public void SetActiveArrowsTurn(bool value) {
        arrowTurnPlayer.GetComponentsInParent<RectTransform>(true)[1].gameObject.SetActive(value);
        arrowTurnOpponent.GetComponentsInParent<RectTransform>(true)[1].gameObject.SetActive(value);
    }

    public IEnumerator ChangeTurn(bool isPlayerTurn) {
        yield return new WaitForSeconds(1f);
        arrowTurnPlayer.SetActive(isPlayerTurn);
        arrowTurnOpponent.SetActive(!isPlayerTurn);
    }

    public void SetLevelBarEnabled(bool value) {
        gameObject.SetActive(value);
    }

    public void SetMoves(int moves, int currentLevelMoves) {
        valueText1.text = moves + " / " + currentLevelMoves;
    }

    public void SetMoves(int moves) {
        valueText1.text = moves + "";
    }

    public void SetTime(float time, float currentLevelTime) {
        valueText2.text = FloatToStringTime(time) + " / " + FloatToStringTime(currentLevelTime);
    }

    public void SetTime(float time) {
        valueText2.text = FloatToStringTime(time);
    }

    public void SetPairs (int pairs) {
        valueText1.text = pairs + "";
    }

    public void SetPlayersMovesDuel(int pairs) {
        valueText1.text = pairs + "";
    }

    public void SetOpponentsMovesDuel(int pairs) {
        valueText2.text = pairs + "";
    }

    public void SetTimer(float time) {
        valueText2.text = FloatToStringTime(time);
    }

    public void SetLockImage(bool value) {
        lockImage.SetActive(value);
    }

    public void InitBarNames(int gameMode) {
        LanguageTitles languageTitles = LanguageTitles.GetInstance();
        switch (gameMode) {
            case 1:
                nameText1.text = languageTitles.movesTitle;
                nameText2.text = languageTitles.timeTitle;
                break;
            case 2:
                nameText1.text = languageTitles.movesTitle;
                nameText2.text = languageTitles.timeTitle;
                break;
            case 3:
                nameText1.text = languageTitles.purrTitle;
                nameText2.text = languageTitles.furrTitle;
                break;
            case 4:
                nameText1.text = languageTitles.scoreTitle;
                nameText2.text = languageTitles.timeTitle;
                break;
        }
    }

    public IEnumerator BonusTimeAnimation(float bonusTime) {
        bonusTimeText.text = "+" + FloatToStringTime(bonusTime);
        bonusTimeText.gameObject.SetActive(true);
        RectTransform textRect = bonusTimeText.rectTransform;
        float startX = textRect.localPosition.x;
        float movingTime = 0;
        Color textColor = bonusTimeText.color;
        yield return new WaitForSeconds(0.5f);
        
        while (textRect.localPosition.x != 0) {
            movingTime += 0.05f;
            float _interpolatedValueX = Mathf.Lerp(startX, 0, movingTime);
            textColor.a = Mathf.Lerp(1, 0, movingTime);
            textRect.localPosition = new Vector3(_interpolatedValueX, -8, 0);
            bonusTimeText.color = textColor;
            yield return new WaitForFixedUpdate();
        }
        bonusTimeText.gameObject.SetActive(false);
        textColor.a = 1;
        bonusTimeText.color = textColor;
        textRect.localPosition = new Vector3(startX, -8, 0);
    }

    public static string FloatToStringTime(float time) {
        int minute = 0;
        float startValue = time;
        while (startValue >= 60) {
            minute++;
            startValue -= 60;
        }
        int s = (int)time % 60;
        string seconds = s +"";
        if (s < 10) {
            seconds = "0" + seconds; 
        }
        return minute + ":" + seconds;
    }
}
