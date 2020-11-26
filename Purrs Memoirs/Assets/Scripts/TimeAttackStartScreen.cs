using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeAttackStartScreen : MonoBehaviour {

    public Image startBackground;
    public Text timeToStartText;

    public void SetActiveScreen (bool value) {
        gameObject.SetActive(value);
    }

    public IEnumerator CountToStart() {
        yield return new WaitForSeconds(0.5f);
        timeToStartText.text = "2";
        yield return new WaitForSeconds(0.4f);
        timeToStartText.text = "1";
        yield return new WaitForSeconds(0.4f);
        timeToStartText.text = LanguageTitles.GetInstance().goTitle;
        yield return new WaitForSeconds(0.3f);

        float time = 0;
        while (startBackground.color.a != 0) {
            time += 0.15f;
            float interpolatedColor = Mathf.Lerp(1, 0, time);
            Color color = startBackground.color;
            color.a = interpolatedColor;
            startBackground.color = color;
            color = timeToStartText.color;
            color.a = interpolatedColor;
            timeToStartText.color = color;
            yield return new WaitForFixedUpdate();
        }
        SetActiveScreen(false);
        FindObjectOfType<Game>().ContinueGame();
    }
}
