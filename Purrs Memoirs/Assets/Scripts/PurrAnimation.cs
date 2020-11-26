using System.Collections;
using UnityEngine;

public class PurrAnimation : MonoBehaviour {

    public Sprite[] purrFrames;
    private SpriteRenderer purrRenderer;

    private void Start() {
        purrRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(PurrAppearance());
    }

    private IEnumerator PurrAppearance() {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < purrFrames.Length; i++) {
            purrRenderer.sprite = purrFrames[i];
            yield return new WaitForSeconds(0.04f);
        }
    }

}
