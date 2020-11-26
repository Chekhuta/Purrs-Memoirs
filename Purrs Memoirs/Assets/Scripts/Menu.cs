using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Menu : MonoBehaviour {

    public OpponentAnimation opponentAnimation;
    public MenuButtons buttons;
    public MenuConveyors conveyors;
    public TasksPanel tasksPanel;
    public QuickGamePanel quickGame;
    public GameObject taskAttentionSign;
    
    private MenuTranslator menuTranslator;
    private bool isButtonsActive = true;
    private bool _appPause = false;
    private bool _socialPress = false;
    private GDPRPopup gdprPopup;

    private void Start() {
        menuTranslator = GetComponent<MenuTranslator>();
        menuTranslator.UpdateTitles();
        TasksData.LoadTasksData();
        if (TasksData.IsCompletedTasks()) {
            SetEnabledTaskSign(true);
        }
        if(!TasksData.IsTutorialCompleted) {
            isButtonsActive = false;
            opponentAnimation.SetCanCount(false);
            FindObjectOfType<Tutorial>().StartTutorial();
        }

        FindObjectOfType<Sounds>().PlayConveyorSound();
        conveyors.ShiftPagesToLeft();
        buttons.OpenTimeAttackButton();
    }

    public void ButtonAction(int action) {
        if (!isButtonsActive) {
            return;
        }
        Sounds.GetInstance().PlayClickSound();
        switch (action) {
            case 1:
                Transition.GetInstance().EndScene("Areas");
                break;
            case 2:
                isButtonsActive = false;
                opponentAnimation.SetCanCount(false);
                quickGame.OpenQuickGamePanel();
                break;
            case 3:
                DuelChallenge();
                break;
            case 4:
                Game.GameMode = 4;
                Transition.GetInstance().EndScene("Game");
                break;
            case 5:
                Social.ShowAchievementsUI();
                break;
            case 6:
                Social.ShowLeaderboardUI();
                break;
            case 7:
                Sounds.GetInstance().PlayConveyorSound();
                buttons.UpdateGPGSButton();
                opponentAnimation.SetCanCount(false);
                FindObjectOfType<TimeAttackButtonAnimation>().CloseBestScore();
                conveyors.ShiftPagesToLeft();
                break;
            case 8:
                isButtonsActive = false;
                opponentAnimation.SetCanCount(false);
                tasksPanel.ShowPanel();
                break;
            case 12:
                buttons.UpdateMusicButton();
                break;
            case 13:
                buttons.UpdateSoundButton();
                    
                DataStorage.SetSound(!DataStorage.Sound);
                break;
            case 14:
                DataStorage.NextLanguage();
                StartCoroutine(LoadTitleFile());
                break;
            case 15:
                Application.OpenURL("https://play.google.com/store/apps/details?id=com.frozenparrot.purrsmemoirs");
                break;
            case 16:
                OpenInstagramURL();
                break;
            case 17:
                Application.OpenURL("https://play.google.com/store/apps/developer?id=Frozen+Parrot");
                break;
            case 18:
                buttons.SwitchGPGSButton();
                break;
            case 19:
                Application.OpenURL("");
                break;
            case 20:
                isButtonsActive = false;
                if (gdprPopup == null) {
                    StartCoroutine(LoadGDPRPopup());
                }
                else {
                    gdprPopup.gameObject.SetActive(true);
                }
                break;
            case 21:
                Sounds.GetInstance().PlayConveyorSound();
                opponentAnimation.SetCanCount(true);
                conveyors.ShiftPagesToRight();
                buttons.OpenTimeAttackButton();
                break;
        }
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
        menuTranslator.UpdateTitles();
        buttons.UpdateLanguageButtonText();
    }

    public void DuelChallenge() {
        Game.GameMode = 3;
        Transition.GetInstance().EndScene("Game");
    }

    public void SetActiveButtons(bool value) {
        isButtonsActive = value;
    }

    public void OpenInstagramURL() {
        if (!_socialPress) {
            _socialPress = true;
            StartCoroutine(OpenSocialURL("", ""));
        }
    }

    public void OpenFacebookURL() {
        if (!_socialPress) {
            _socialPress = true;
            StartCoroutine(OpenSocialURL("", ""));
        }
    }

    private void OnApplicationPause(bool pauseStatus) {
        if (pauseStatus) {
            _appPause = true;
        }
    }

    private IEnumerator OpenSocialURL(string appURL, string browserURL) {
        Application.OpenURL(appURL);
        yield return new WaitForSeconds(1);
        if (_appPause) {
            _appPause = false;
        }
        else {
            Application.OpenURL(browserURL);
        }
        _socialPress = false;
    }

    public void SetEnabledTaskSign(bool enabled) {
        taskAttentionSign.SetActive(enabled);
    }
}
