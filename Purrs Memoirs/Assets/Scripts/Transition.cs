using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour {

    public Image leftPart;
    public Image rightPart;
    private string scene = "";
    private BackgroundMusic backgroundMusic;
    private static Transition instance;

    public static Transition GetInstance() {
        return instance;
    }

    private void Awake() {
        instance = this;
    }

    private void Start() {
        StartScene();
    }

    public void StartScene() {
        backgroundMusic = FindObjectOfType<BackgroundMusic>();
        gameObject.SetActive(true);
        GetComponent<Animator>().SetTrigger("Open");
    }

    public void EndScene(string sceneName) {
        if (backgroundMusic) {
            backgroundMusic.StartTransition();
        }
        gameObject.SetActive(true);
        GetComponent<Animator>().SetTrigger("Close");
        scene = sceneName;
    }

    public void LoadScene() {
        if (scene != "") {
            SceneManager.LoadScene(scene);
        }
    }

    public void UpdatePartsSize(float partHeight) {
        leftPart.rectTransform.sizeDelta = new Vector2(leftPart.rectTransform.sizeDelta.x, partHeight);
        rightPart.rectTransform.sizeDelta = new Vector2(rightPart.rectTransform.sizeDelta.x, partHeight);
    }
}
