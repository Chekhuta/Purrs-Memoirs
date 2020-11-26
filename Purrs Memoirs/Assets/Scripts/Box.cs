using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Box : MonoBehaviour, IPointerDownHandler {

    public Sprite[] boxFrames;
    public bool CanBeSelected { get; set; } = true;
    public int BoxId { get; set; }
    public int ContentValue { get; set; }
    public AudioClip openBoxSound;
    public AudioClip closeBoxSound;
    private Sprite[] catFrames;
    private SpriteRenderer catRenderer;
    private bool isOpened = false;
    private bool isEmpty = false;
    private SpriteRenderer boxRenderer;
    private AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        boxRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (!isEmpty && !isOpened) {
            OpenBox();
        }
    }

    public void OpenBox() {
        if (!CanBeSelected) {
            return;
        }
        OpenBoxAnimated();
    }

    public void OpenBoxAnimated() {
        isOpened = true;
        PlayOpenBoxSound();
        StartCoroutine(AnimatedOpenBox());
        Game.GetInstance().MakePair(BoxId);
    }

    public void InitializeBoxContent(int id, int content, GameObject catFramesObject) {
        catRenderer = catFramesObject.GetComponent<SpriteRenderer>();
        catFrames = catFramesObject.GetComponent<CatFrames>().GetCatFrames();
        BoxId = id;
        ContentValue = content;
        catRenderer.gameObject.SetActive(false);
    }

    public void CloseBox() {
        isOpened = false;
        PlayCloseBoxSound();
        StartCoroutine(AnimatedCloseBox());
    }

    public bool IsEmpty() {
        return isEmpty;
    }

    public void MarkBoxAsEmpty() {
        isEmpty = true;
    }

    private IEnumerator AnimatedOpenBox() {
        int catFrameIndex = 0;
        for (int i = 1; i < boxFrames.Length; i++) {
            boxRenderer.sprite = boxFrames[i];

            if (i == 3) {
                if (ContentValue == 7) {
                    boxRenderer.enabled = false; ;
                }
                catRenderer.gameObject.SetActive(true);
                catRenderer.sprite = catFrames[catFrameIndex];
                catFrameIndex++;
            }
            else if (i > 3) {
                catRenderer.sprite = catFrames[catFrameIndex];
                catFrameIndex++;
            }
            yield return new WaitForSeconds(0.04f);
        }

        for (int i = catFrameIndex; i < catFrames.Length; i++) {
            catRenderer.sprite = catFrames[i];
            yield return new WaitForSeconds(0.04f);
        }
    }

    private IEnumerator AnimatedCloseBox() {
        int boxFrameIndex = boxFrames.Length - 2;
        for (int i = catFrames.Length - 2; i >= 0; i--) {
            catRenderer.sprite = catFrames[i];

            if (i < 3) {
                if (ContentValue == 7) {
                    boxFrameIndex--;
                }
                else {
                    boxRenderer.sprite = boxFrames[boxFrameIndex];
                    boxFrameIndex--;
                }
        
            }

            yield return new WaitForSeconds(0.04f);
        }
        if (ContentValue == 7) {
            boxRenderer.enabled = true;
        }
        catRenderer.gameObject.SetActive(false);

        for (int i = boxFrameIndex; i >= 0; i--) {
            boxRenderer.sprite = boxFrames[i];
            yield return new WaitForSeconds(0.04f);
        }
        Game.GetInstance().UnblockBoxesAfterClosing();
    }

    private void PlayOpenBoxSound() {
        if (DataStorage.Sound) {
            audioSource.PlayOneShot(openBoxSound, 1f);
        }
    }

    private void PlayCloseBoxSound() {
        if (DataStorage.Sound) {
            audioSource.PlayOneShot(closeBoxSound, 1f);
        }
    }
}
