using UnityEngine;
using UnityEngine.UI;
using Firebase.Analytics;

public class LevelCompletePanel : MonoBehaviour {

    public Transition transition;
    public Animator levelComplete;
    public GameObject background;
    public Text headerText;
    public Text scoreText;
    public Text bestScoreText;
    public Text movesText;
    public Text timeText;
    public Text continueText;
    public Button continueCoinButton;
    public Button continueAdButton;
    public Image[] stars;
    public Sprite star;
    public Sprite starPlaceholder;
    public TaskCompletedBoard taskCompletedBoard;
    private bool isAdRefused = false;
    private bool isGameContinued = false;
    private int panelShowCount = 0;
    private GameScore gameScore;
    private int continueGamePrice = 300;
    private int scoreBeforeContinue = 0;

    public void ShowPanel(int gameMode) {
        LanguageTitles languageTitles = LanguageTitles.GetInstance();
        switch (gameMode) {
            case 1:
                headerText.fontSize = 35;
                headerText.text = languageTitles.completedTitle;

                gameScore = FindObjectOfType<GameScore>();
                movesText.text = languageTitles.movesTitle + "\n" + gameScore.ScoreValue + " / " + CurrentLevelParameters.TopLevelMoves;
                timeText.text = languageTitles.timeTitle + "\n" + UILevelBar.FloatToStringTime(gameScore.LevelTime) + " / " + UILevelBar.FloatToStringTime(CurrentLevelParameters.TopLevelTime);

                int goldStarCount = DataStorage.GetCountOfStars(Game.LevelNumber);
                for (int i = 0; i < stars.Length; i++) {
                    if (i < goldStarCount) {
                        stars[i].sprite = star;
                    }
                    else {
                        stars[i].sprite = starPlaceholder;
                    }
                }
                gameObject.SetActive(true);
                levelComplete.SetTrigger("Paper 1");
                break;
            case 2:
                headerText.fontSize = 35;
                headerText.text = languageTitles.completedTitle;
                gameObject.SetActive(true);
                levelComplete.SetTrigger("Paper 2");
                break;
            case 3:
                headerText.fontSize = 35;
                gameScore = FindObjectOfType<GameScore>();
                string resultParameter;
                if (gameScore.PlayerDuelScore == gameScore.OpponentDuelScore) {
                    headerText.text = languageTitles.drawTitle;
                    resultParameter = "draw";
                }
                else if (gameScore.PlayerDuelScore > gameScore.OpponentDuelScore) {
                    headerText.text = languageTitles.youWinTitle;
                    resultParameter = "win";
                }
                else {
                    headerText.text = languageTitles.youLoseTitle;
                    resultParameter = "lose";
                }
                FirebaseAnalytics.LogEvent("duel_level", "result", resultParameter);
                gameObject.SetActive(true);
                levelComplete.SetTrigger("Paper 2");
                break;
            case 4:
                if (DataStorage.LanguageId != 0) {
                    headerText.fontSize = 35;
                }
                headerText.text = languageTitles.timesUpTitle;
                gameScore = FindObjectOfType<GameScore>();
                continueText.text = languageTitles.continueTitle;
                scoreText.text = languageTitles.scoreTitle + ": " + gameScore.ScoreValue;
                bestScoreText.text = languageTitles.bestScoreTitle + ": " + DataStorage.TimeAttackScore;
                panelShowCount++;
                if (panelShowCount == 2) {
                    isGameContinued = false;
                }
                gameObject.SetActive(true);
                levelComplete.SetTrigger("Time Attack Zero");
                break;
        }
        if (TasksData.IsCompletedTasks()) {
            taskCompletedBoard.ShowBoard();
        }
    }

    public void ShowAd() {
        if (isGameContinued) {
            return;
        }
        FindObjectOfType<AdManager>().DisplayRewardVideo(this);
    }

    public void PayForContinueGame() {
        if (isGameContinued) {
            return;
        }
        if (TasksData.Coins >= continueGamePrice) {
            TasksData.PayToContinueGame(continueGamePrice);
            FirebaseAnalytics.LogEvent("pay_coins_continue");
            FindObjectOfType<CoinField>().UpdateCoinField();
            ContinueGame();
        }
    }

    public void ContinueGame() {
        isGameContinued = true;
        levelComplete.SetTrigger("Remove Ad");
        FindObjectOfType<Game>().ContinueGameAfterAd();
    }

    public void DeactivatePanel() {
        if (isGameContinued) {
            gameObject.SetActive(false);
        }
    }

    public void ShowButtons() {
        if (!isAdRefused && !isGameContinued) {
            switch (Game.GameMode) {
                case 1:
                    levelComplete.SetTrigger("Buttons 1");
                    break;
                case 2:
                    levelComplete.SetTrigger("Buttons 2");
                    break;
                case 3:
                    levelComplete.SetTrigger("Buttons 2");
                    break;
                case 4:
                    if (panelShowCount == 2) {
                        FirebaseAnalytics.LogEvent("time_level_continue", "score", gameScore.ScoreValue - scoreBeforeContinue);
                        levelComplete.SetTrigger("Show Score");
                    }
                    else {
                        bool coinButtonInteractable = TasksData.Coins >= continueGamePrice;
                        bool adButtonInteractable = FindObjectOfType<AdManager>().IsLoadRewardVideo();
                        FirebaseAnalytics.LogEvent("time_level", "score", gameScore.ScoreValue);
                        if (gameScore.ScoreValue >= 10 && (coinButtonInteractable || adButtonInteractable)) {
                            scoreBeforeContinue = gameScore.ScoreValue;
                            continueCoinButton.interactable = coinButtonInteractable;
                            continueCoinButton.GetComponentInChildren<Text>().text = continueGamePrice + "";
                            continueAdButton.interactable = adButtonInteractable;
                            levelComplete.SetTrigger("Ad");
                        }
                        else {
                            levelComplete.SetTrigger("Show Score");
                        }
                    }
                    break;
            }
        }
    }

    public void RefuseAdPanel() {
        if (isGameContinued) {
            return;
        }
        isAdRefused = true;
        levelComplete.SetTrigger("Remove Ad");
    }

    public void ShowScorePanel() {
        if (isAdRefused) {
            levelComplete.SetTrigger("Show Score");
        }
        else if (isGameContinued) {
            headerText.text = LanguageTitles.GetInstance().goTitle;
            levelComplete.SetTrigger("Continue Game");
        }
    }

    public void ReturnToMenu() {
        FindObjectOfType<AdManager>().DisplayInterstitial();
        transition.EndScene("Menu");
    }

    public void RestartGame() {
        FindObjectOfType<AdManager>().DisplayInterstitial();
        transition.EndScene("Game");
    }

    public void PlayNextLevel() {
        FindObjectOfType<AdManager>().DisplayInterstitial();
        if (Game.GameMode == 1) {
            if (Game.LevelNumber == 30) {
                transition.EndScene("Menu");
            }
            else {
                Game.LevelNumber++;
                SelectLevel.AreaLevels[Game.LevelNumber - 1].SetLevelParameters();
                transition.EndScene("Game");
            }
        }
        else {
            transition.EndScene("Game");
        }
    }

    public GameObject GetBackground() {
        return background;
    }
}
