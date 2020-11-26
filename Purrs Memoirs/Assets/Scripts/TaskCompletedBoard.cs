using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TaskCompletedBoard : MonoBehaviour {

    public Text taskCompletedText;

    public void ShowBoard() {
        gameObject.SetActive(true);
        taskCompletedText.text = LanguageTitles.GetInstance().taskCompletedTitle;
        GetComponent<RectTransform>().localScale = new Vector3(0, 0, 1);
        StartCoroutine(ShowAnimation());
    }

    private IEnumerator ShowAnimation() {
        float movingTime = 0;
        RectTransform boardRect = GetComponent<RectTransform>();
        yield return new WaitForSeconds(0.2f);
        while (boardRect.localScale.y != 2) {
            movingTime += 0.12f;
            float _interpolatedScaleValue = Mathf.Lerp(0, 2, movingTime);
            boardRect.localScale = new Vector3(_interpolatedScaleValue, _interpolatedScaleValue, 1);
            yield return new WaitForFixedUpdate();
        }
    }
}
