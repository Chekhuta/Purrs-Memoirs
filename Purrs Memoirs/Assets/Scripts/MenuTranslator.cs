using UnityEngine;
using UnityEngine.UI;

public class MenuTranslator : MonoBehaviour {

    public TextMesh campaignText;
    public TextMesh campaignTextShadow;
    public TextMesh quickGameText;
    public TextMesh quickGameTextShadow;
    public TextMesh duelText;
    public TextMesh duelTextShadow;
    public TextMesh timeAttackText;
    public TextMesh timeAttackTextShadow;
    public TextMesh bestScoreText1;
    public TextMesh bestScoreTextShadow1;
    public TextMesh bestScoreText2;
    public TextMesh bestScoreTextShadow2;
    public Text sizeText;
    public Text cyclesText;
    public Text quickGamePaperText;
    public Text yesText;

    private void Start() {
        UpdateTitles();
    }

    public void UpdateTitles() {
        LanguageTitles languageTitles = LanguageTitles.GetInstance();
        campaignText.text = languageTitles.campaignTitle;
        campaignTextShadow.text = languageTitles.campaignTitle;
        quickGameText.text = languageTitles.quickGameTitle;
        quickGameTextShadow.text = languageTitles.quickGameTitle;
        duelText.text = languageTitles.duelTitle;
        duelTextShadow.text = languageTitles.duelTitle;
        timeAttackText.text = languageTitles.timeAttackTitle;
        timeAttackTextShadow.text = languageTitles.timeAttackTitle;
        sizeText.text = languageTitles.sizeTitle;
        cyclesText.text = languageTitles.cyclesTitle;
        bestScoreText1.text = languageTitles.bestTitle + ": " + DataStorage.TimeAttackScore;
        bestScoreTextShadow1.text = languageTitles.bestTitle + ": " + DataStorage.TimeAttackScore;
        bestScoreText2.text = languageTitles.bestTitle + ": " + DataStorage.TimeAttackScore;
        bestScoreTextShadow2.text = languageTitles.bestTitle + ": " + DataStorage.TimeAttackScore;
        quickGamePaperText.text = languageTitles.quickGamePaperTitle;
        yesText.text = languageTitles.yesTitle;
    }

}
