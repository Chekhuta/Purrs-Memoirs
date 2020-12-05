using UnityEngine;

public class GameScore : MonoBehaviour{

    public UILevelBar levelBar;
    public int ScoreValue { get; set; } = 0;
    public float LevelTime { get; set; } = 0;
    public bool Pause { get; set; } = false;
    public int PlayerDuelScore { get; set; } = 0;
    public int OpponentDuelScore { get; set; } = 0;
    public bool IsTimeFreeze { get; set; } = false;

    private void Update() {
        if (!Pause && !IsTimeFreeze) {
            if (Game.GameMode == 4) {
                LevelTime -= Time.deltaTime;
                levelBar.SetTimer(LevelTime);
                if (LevelTime <= 1) {
                    Pause = true;
                    FindObjectOfType<Game>().GameOver();
                }
            }
            else if (Game.GameMode == 1) {
                LevelTime += Time.deltaTime;
                levelBar.SetTime(LevelTime, CurrentLevelParameters.TopLevelTime);
            }
            else if (Game.GameMode == 2) {
                LevelTime += Time.deltaTime;
                levelBar.SetTime(LevelTime);
            }
        }
    }

    public void InitGameScore(int gameMode) {
        levelBar.InitBarNames(gameMode);
        switch(gameMode) {
            case 0:
                levelBar.SetLevelBarEnabled(false);
                break;
            case 1:
                levelBar.SetMoves(ScoreValue, CurrentLevelParameters.TopLevelMoves);
                levelBar.SetTime(0, CurrentLevelParameters.TopLevelTime);
                break;
            case 2:
                levelBar.SetMoves(ScoreValue);
                levelBar.SetTime(0);
                break;
            case 3:
                levelBar.SetOpponentsMovesDuel(0);
                levelBar.SetPlayersMovesDuel(0);
                break;
            case 4:
                levelBar.SetPairs(ScoreValue);
                LevelTime = 15;
                levelBar.SetTimer(LevelTime);
                break;
        }
    }

    public void AddPairsScore() {
        ScoreValue++;
        levelBar.SetPairs(ScoreValue);
    }

    public void AddMovesScore() {
        ScoreValue++;
        if (Game.GameMode == 1) {
            levelBar.SetMoves(ScoreValue, CurrentLevelParameters.TopLevelMoves);
        }
        else if (Game.GameMode == 2){
            levelBar.SetMoves(ScoreValue);
        }
    }

    public void AddPlayerDuelScore() {
        PlayerDuelScore++;
        levelBar.SetPlayersMovesDuel(PlayerDuelScore);
    }

    public void AddOpponentDuelScore() {
        OpponentDuelScore++;
        levelBar.SetOpponentsMovesDuel(OpponentDuelScore);
    }

    public void AddTime(float bonusTime) {
        LevelTime += bonusTime;
        levelBar.SetTimer(LevelTime);
        StartCoroutine(levelBar.BonusTimeAnimation(bonusTime));
    }

    public void FreezeTime() {
        IsTimeFreeze = true;
        LevelTime = 10;
        levelBar.SetTimer(LevelTime);
        levelBar.SetLockImage(true);
    }

    public void UnfreezeTime() {
        levelBar.SetLockImage(false);
        IsTimeFreeze = false;
    }
}
