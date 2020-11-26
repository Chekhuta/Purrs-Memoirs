using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Logo : MonoBehaviour {

    public Texture fontTexture;

    public SpriteRenderer catRenderer;
    public Sprite[] catFrames;

    public SpriteRenderer logoRenderer;
    public Sprite[] logoFrames;

    public SpriteRenderer blinkRenderer;
    public Sprite[] blinkFrames;
    public AudioSource catAudioSource;

    public Transition transition;
    private GDPRPopup gdprPopup;
    private ConsentCheck consentCheck;
    private AudioSource audioSource;
    private bool isPopupOpened;


    private void Start() {
        fontTexture.filterMode = FilterMode.Point;
        audioSource = GetComponent<AudioSource>();
        consentCheck = GetComponent<ConsentCheck>();
        DataStorage.LoadDataStorage();
        consentCheck.CheckGDPROnLogo(this);
        StartCoroutine(ShowLogo());
    }

    private IEnumerator CheckPopupEnabled() {

        yield return new WaitForSeconds(1.4f);

        if (!isPopupOpened) {
            GPGSAuthentication.InitializeGPGS();
            LoadMenuScene();
        }
    }

    private IEnumerator ShowLogo() {

        yield return new WaitForSeconds(1);

        for (int i = 0; i < catFrames.Length; i++) {
            catRenderer.sprite = catFrames[i];
            if (i == 3) {
                if (DataStorage.Sound) {
                    catAudioSource.Play();
                }
            }
            yield return new WaitForSeconds(0.08f);
        }

        catRenderer.sprite = catFrames[1];
        logoRenderer.sprite = logoFrames[0];
        yield return new WaitForSeconds(0.08f);

        catRenderer.sprite = catFrames[0];
        logoRenderer.sprite = logoFrames[1];
        yield return new WaitForSeconds(0.08f);

        catRenderer.gameObject.SetActive(false);
        logoRenderer.sprite = logoFrames[2];
        yield return new WaitForSeconds(0.08f);

        for (int i = 3; i < logoFrames.Length; i++) {
            logoRenderer.sprite = logoFrames[i];
            yield return new WaitForSeconds(0.08f);
        }

        if (DataStorage.Sound) {
            audioSource.Play();
        }

        for (int i = 0; i < blinkFrames.Length; i++) {
            blinkRenderer.sprite = blinkFrames[i];
            yield return new WaitForSeconds(0.08f);
        }

        blinkRenderer.gameObject.SetActive(false);
        yield return StartCoroutine(LoadTitleFile());
        StartCoroutine(CheckPopupEnabled());
    }

    public void LoadMenuScene() {
        transition.EndScene("Menu");
    }

    public void ShowGDPRPopup() {
        isPopupOpened = true;
        StartCoroutine(LoadGDPRPopup());
    }

    private IEnumerator LoadGDPRPopup() {
        AssetReference gdprPopupReference = new AssetReference("Assets/Prefabs/GDPRCanvas.prefab");
        AsyncOperationHandle gdprPopupOperationHandle = gdprPopupReference.LoadAssetAsync<GameObject>();
        yield return gdprPopupOperationHandle;
        gdprPopup = Instantiate((GameObject)gdprPopupOperationHandle.Result).GetComponent<GDPRPopup>();
        Addressables.Release(gdprPopupOperationHandle);
    }

    private IEnumerator LoadTitleFile() {
        AssetReference titlesFileReference = new AssetReference("Assets/Titles/titles_" + DataStorage.GetLanguageString() + ".json");
        AsyncOperationHandle titlesFileOperationHandle = titlesFileReference.LoadAssetAsync<TextAsset>();
        yield return titlesFileOperationHandle;
        LanguageTitles.SetInstance(JsonUtility.FromJson<LanguageTitles>(((TextAsset)titlesFileOperationHandle.Result).text));
        Addressables.Release(titlesFileOperationHandle);
    }

}
