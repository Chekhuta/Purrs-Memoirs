using UnityEngine;

public class MenuButtons : MonoBehaviour {

    public Sprite[] soundButtonSprites;
    public Sprite[] musicButtonSprites;
    public Sprite[] gpgsButtonSprites;
    public SpriteRenderer soundButton;
    public SpriteRenderer musicButton;
    public SpriteRenderer gpgsButton;
    public GameObject gdprButton;
    public TextMesh languageButtonText;
    public TextMesh languageButtonTextShadow;
    public TextMesh moreGamesButtonText;
    public TextMesh moreGamesButtonTextShadow;
    public TextMesh soonShopButtonText;
    public TextMesh soonShopButtonTextShadow;
    public TimeAttackButtonAnimation timeAttackButton;

    private void Start() {
        UpdateLanguageButtonText();
        if (!DataStorage.IsEEA) {
            gdprButton.SetActive(false);
        }
        if (!DataStorage.Music) {
            musicButton.sprite = musicButtonSprites[1];
        }
        if (!DataStorage.Sound) {
            soundButton.sprite = soundButtonSprites[1];
        }
    }

    public void UpdateLanguageButtonText() {
        LanguageTitles languageTitles = LanguageTitles.GetInstance();
        languageButtonText.text = languageTitles.languageButtonTitle;
        languageButtonTextShadow.text = languageTitles.languageButtonTitle;
        moreGamesButtonText.text = languageTitles.moreGamesTitle;
        moreGamesButtonTextShadow.text = languageTitles.moreGamesTitle;
        soonShopButtonText.text = languageTitles.soonTitle;
        soonShopButtonTextShadow.text = languageTitles.soonTitle;
    }

    public void UpdateGPGSButton() {
        if (!GPGSAuthentication.IsInitialize()) {
            gpgsButton.sprite = gpgsButtonSprites[1];
        }
    }

    public void SwitchGPGSButton() {
        if (gpgsButton.sprite == gpgsButtonSprites[0]) {
            GPGSAuthentication.SignOut();
            gpgsButton.sprite = gpgsButtonSprites[1];
        }
        else {
            GPGSAuthentication.InitializeGPGS();
            gpgsButton.sprite = gpgsButtonSprites[0];
        }
    }

    public void UpdateSoundButton() {
        if (soundButton.sprite == soundButtonSprites[0]) {
            soundButton.sprite = soundButtonSprites[1];
        }
        else {
            soundButton.sprite = soundButtonSprites[0];
        }
    }
    
    public void UpdateMusicButton() {
        if (DataStorage.Music) {
            musicButton.sprite = musicButtonSprites[1];
            BackgroundMusic.GetInstance().StopMusic();
            DataStorage.SetMusic(false);
        }
        else {
            musicButton.sprite = musicButtonSprites[0];
            BackgroundMusic.GetInstance().PlayMusic();
            DataStorage.SetMusic(true);
        }
    }

    public void OpenTimeAttackButton() {
        timeAttackButton.OpenBestScore();
    }

}
