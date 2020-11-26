using System.Collections;
using UnityEngine;

public class OpponentAnimation : MonoBehaviour {

    public Sprite[] opponentFrames;
    public SpriteRenderer duelButton;
    private Transform duelButtonTransform;
    private SpriteRenderer opponentRenderer;
    private float AFKTime = 0;
    private bool canCount = true;

    private void Start() {
        opponentRenderer = GetComponent<SpriteRenderer>();
        duelButtonTransform = duelButton.transform;
    }

    private void Update() {
        if (!canCount) {
            return;
        }
        AFKTime += Time.deltaTime;
        if (AFKTime >= 90 && canCount) {
            canCount = false;
            StartCoroutine(PlayOpponentAnimation());
        }
    }

    public void SetCanCount(bool value) {
        canCount = value;
        if (value) {
            AFKTime = 0;
        }
    }

    private IEnumerator PlayOpponentAnimation() {
        Menu menu = FindObjectOfType<Menu>();
        menu.SetActiveButtons(false);
        Social.ReportProgress(GPGSIds.achievement_its_high_noon, 100f, null);
        opponentRenderer.sprite = opponentFrames[0];
        yield return new WaitForSeconds(0.1f);
        opponentRenderer.sprite = opponentFrames[1];
        yield return new WaitForSeconds(0.6f);

        for (int i = 2; i < opponentFrames.Length; i++) {
            opponentRenderer.sprite = opponentFrames[i];
            yield return new WaitForSeconds(0.06f);
        }
        duelButtonTransform.localPosition -= new Vector3(0, 0.02f, 0);
        duelButton.size -= new Vector2(0, 0.02f);
        yield return new WaitForSeconds(1f);
        menu.DuelChallenge();
    }
}
