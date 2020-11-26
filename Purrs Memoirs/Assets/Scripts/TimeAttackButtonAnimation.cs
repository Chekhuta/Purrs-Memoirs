using System.Collections;
using UnityEngine;

public class TimeAttackButtonAnimation : MonoBehaviour {

    public GameObject[] bestScoreFrames;
    public TextMesh[] textFrame3;
    public TextMesh[] textFrame4;
    private int currentFrame = 0;
    private bool isOpen = false;

    public void OpenBestScore() {
        if (isOpen) {
            return;
        }
        isOpen = true;
        StartCoroutine(ShowBestScoreAnimation());
    }

    public void CloseBestScore() {
        if (currentFrame == 3) {
            isOpen = false;
            StartCoroutine(CloseBestScoreAnimation());
        }
    }

    private IEnumerator ShowBestScoreAnimation() {
        yield return new WaitForSeconds(0.4f);
        for (int i = 1; i < bestScoreFrames.Length; i++) {
            bestScoreFrames[i - 1].SetActive(false);
            bestScoreFrames[i].SetActive(true);
            currentFrame = i;
            yield return new WaitForSeconds(0.04f);
        }
    }

    private IEnumerator CloseBestScoreAnimation() {
        for (int i = 2; i >= 0; i--) {
            bestScoreFrames[i + 1].SetActive(false);
            bestScoreFrames[i].SetActive(true);
            currentFrame = i;
            yield return new WaitForSeconds(0.04f);
        }
    }
}
