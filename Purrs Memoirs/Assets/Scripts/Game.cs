using System.Collections;
using UnityEngine;
using GooglePlayGames;
using Firebase.Analytics;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Game : MonoBehaviour {

    public static int LevelNumber { get; set; }
    public static int GameMode { get; set; }
    public TimeAttackStartScreen timeAttackScreen;
    public LevelCompletePanel levelComplete;
    public RatePopup ratePopup;
    public AudioClip clickSound;
    public AudioClip pairSound;
    public AudioSource conveyorAudioSource;
    private Transform[] boxes;
    private Level currentLevel;
    private ConveyorsFieldGenerator levelGenerator;
    private GameScore gameScore;
    private DuelOpponent duelOpponent;
    private AudioSource audioSource;
    private Tutorial tutorial;
    private LevelEnvironment levelEnvironment;
    private int firstBoxId = -1;
    private int secondBoxId = -1;
    private int subLevel = 0;
    private bool isPlayerTurn = true;
    private int closeBoxCount = 2;
    private int pairsCount = 0;
    private static Game instance;

    private void Start() {
        instance = this;
        levelEnvironment = GetComponent<LevelEnvironment>();
        audioSource = GetComponent<AudioSource>();
        gameScore = GetComponent<GameScore>();
        gameScore.InitGameScore(GameMode);
        if (GameMode == 0) {
            tutorial = FindObjectOfType<Tutorial>();
            tutorial.SetMessageParent(levelComplete.transform);
            tutorial.SetMessageBackground(Instantiate(levelComplete.GetBackground(), levelComplete.GetComponent<Transform>()));
        }
        if (GameMode != 4 && GameMode != 0) {
            FindObjectOfType<AdManager>().IsInGame = true;
        }
        if (GameMode == 4) {
            gameScore.Pause = true;
            timeAttackScreen.SetActiveScreen(true);
            StartCoroutine(timeAttackScreen.CountToStart());
        }
        levelGenerator = GetComponent<ConveyorsFieldGenerator>();
        levelGenerator.InitConveyorsPositions(GameMode, subLevel);
        currentLevel = levelGenerator.AddLevel();
        boxes = GetComponent<BoxSpawner>().SpawnBoxes(currentLevel, new Vector3(14, 0, -10), true);
        levelEnvironment.InitializeLevelEnvironment(GameMode, currentLevel.GetRowsCount() - 2);

        if (GameMode == 3) {
            StartCoroutine(LoadDuelOpponent());
            UILevelBar.GetInstance().SetActiveArrowsTurn(true);
        }
    }

    public static Game GetInstance() {
        return instance;
    }

    public void MakePair(int boxId) {
        if (firstBoxId == -1) {
            firstBoxId = boxId;
            return;
        }
        BlockBoxes(true);
        secondBoxId = boxId;
        Box firstBox = boxes[firstBoxId].GetComponent<Box>();
        Box secondBox = boxes[secondBoxId].GetComponent<Box>();
        if (GameMode != 4 && GameMode != 3) {
            gameScore.AddMovesScore();
        }

        if (firstBox.ContentValue == secondBox.ContentValue) {
            PlayPairSound();
            if (isPlayerTurn) { 
                pairsCount++;
                TasksData.UpdateTaskProgress(new FindCatTaskType(TaskTypeName.FindCatTask, firstBox.ContentValue), 1);
                TasksData.UpdateTaskProgress(new TaskType(TaskTypeName.FindPairsTask), 1);
            }
            firstBox.MarkBoxAsEmpty();
            secondBox.MarkBoxAsEmpty();
            firstBoxId = -1;
            secondBoxId = -1;
            if (GameMode == 3) {
                if (isPlayerTurn) {
                    gameScore.AddPlayerDuelScore();
                }
                else {
                    gameScore.AddOpponentDuelScore();
                }
                duelOpponent.DeleteEmptyBoxes(firstBox.BoxId, secondBox.BoxId);
            }

            if (!currentLevel.IsCycle() && !gameScore.Pause && isPlayerTurn) {
                BlockBoxes(false);
            }

            if (GameMode == 4) {
                gameScore.AddPairsScore();
            }

            if (IsLevelOver()) {
                switch(GameMode) {
                    case 0:
                        if (subLevel == 0) {
                            StartCoroutine(ContinueTutorial(1));
                        }
                        else {
                            StartCoroutine(ContinueTutorial(2));
                        }
                        break;
                    case 1:
                        TasksData.UpdateTaskProgress(new FieldTaskType(TaskTypeName.FieldTask, currentLevel.GetRowsCount(), currentLevel.GetNumberOfBoxesOnRow(0)), 1);
                        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, new Parameter(FirebaseAnalytics.ParameterLevelName, "campaign_level_" + currentLevel.levelNumber));
                        GameOver();
                        break;
                    case 2:
                        TasksData.UpdateTaskProgress(new FieldTaskType(TaskTypeName.FieldTask, currentLevel.GetRowsCount(), currentLevel.GetNumberOfBoxesOnRow(0)), 1);
                        GameOver();
                        break;
                    case 3:
                        if (gameScore.PlayerDuelScore > gameScore.OpponentDuelScore) {
                            TasksData.UpdateTaskProgress(new TaskType(TaskTypeName.DuelTask), 1);
                        }
                        GameOver();
                        break;
                    case 4:
                        NextSubLevel();
                        int bonusTimeForLevel = LevelParametersGenerator.CurrentLevelParameters.BonusLevelTime;
                        if (currentLevel.IsCycle()) {
                            bonusTimeForLevel += currentLevel.GetBoxesIdForCycle(0).Length / 2;
                        }
                        gameScore.AddTime(bonusTimeForLevel);
                        if (gameScore.IsTimeFreeze) {
                            gameScore.UnfreezeTime();
                        }
                        break;
                }
                return;
            }

            if (GameMode == 3 && !isPlayerTurn && !currentLevel.IsCycle()) {
                DuelOpponentMove();
            }
        }
        else {
            if (GameMode == 3) {
                isPlayerTurn = !isPlayerTurn;
                duelOpponent.AddToMemory(firstBox, secondBox);
                StartCoroutine(UILevelBar.GetInstance().ChangeTurn(isPlayerTurn));
            }
            StartCoroutine(CloseBoxes(firstBox, secondBox));
        }

        if (currentLevel.IsCycle()) {
            StartCoroutine(MoveBoxesOnConveyor());
        }
    }

    public void PauseGame() {
        BackgroundMusic.GetInstance().MuffleMusic();
        gameScore.Pause = true;
        BlockBoxes(true);
    }

    public void ContinueGame() {
        gameScore.Pause = false;
        BlockBoxes(false);
        BackgroundMusic.GetInstance().ContinueMusicAfterPause();
    }

    public void GameOver() {
        gameScore.Pause = true;
        BlockBoxes(true);
        if (DataStorage.RateGameShowCount < 3 && !RatePopup.WasPopupShowed) {
            RatePopup.GamePlayedDelay++;
        }

        if (GameMode != 4) {
            float delay = 0;

            int boxCount = currentLevel.GetCountOfBoxes();

            if (boxCount > 30) {
                delay = 1.5f;
            }
            else if (boxCount > 8) {
                delay = 1f;
            }
            else {
                delay = 0.6f;
            }

            FindObjectOfType<AdManager>().IncreaseDelay(delay);
        }

        switch (GameMode) {
            case 1:
                int stars = 1;
                if (gameScore.LevelTime <= CurrentLevelParameters.TopLevelTime + 1) {
                    stars++;
                }
                if (gameScore.ScoreValue <= CurrentLevelParameters.TopLevelMoves) {
                    stars++;
                }
                if (DataStorage.GetCountOfStars(LevelNumber) < stars) {
                    DataStorage.SetStarsForLevel(LevelNumber, stars);
                }
                break;
            case 4:
                TasksData.UpdateTaskProgress(new TaskType(TaskTypeName.TimeAttackTask), gameScore.ScoreValue);
                if (gameScore.ScoreValue >= 10) {
                    FindObjectOfType<AdManager>().IncreaseAttackTimeGamePlayed();
                }
                break;
        }
        TasksData.SaveTasksData();
        CheckAchievements();
        if (DataStorage.RateGameShowCount < 3 && RatePopup.GamePlayedDelay >= 5 && !RatePopup.WasPopupShowed) {
            ratePopup.ShowPopup();
        }
        else {
            ShowLevelCompletePanel();
        }
    }

    public void ShowLevelCompletePanel() {
        levelComplete.ShowPanel(GameMode);
    }

    private IEnumerator ContinueTutorial(int step) {
        yield return new WaitForSeconds(0.4f);
        if (step == 1) {
            tutorial.ContinueTutorial();
        }
        else if (step == 2) {
            tutorial.ConfirmButtonPressed();
        }
    }

    public void ContinueGameAfterAd() {
        gameScore.FreezeTime();
        gameScore.Pause = false;
        pairsCount = 0;
        BlockBoxes(false);
    }

    public int GetBoxValue(int boxId) {
        return boxes[boxId].GetComponent<Box>().ContentValue;
    }

    public void UnblockBoxesAfterClosing() {
        closeBoxCount--;
        if (closeBoxCount > 0) {
            return;
        }
        closeBoxCount = 2;
        if (!currentLevel.IsCycle() && !gameScore.Pause && isPlayerTurn) {
            BlockBoxes(false);
        }
    }

    private void DuelOpponentMove() {
        int[] id = duelOpponent.TurnBoxesId(this);
        StartCoroutine(duelOpponent.OpenBoxes(boxes[id[0]].GetComponent<Box>(), boxes[id[1]].GetComponent<Box>()));
    }

    public void NextSubLevel() {
        Transform[] oldBoxes = boxes;
        Level oldLevel = currentLevel;
        subLevel++;
        levelGenerator.InitConveyorsPositions(GameMode, subLevel);
        currentLevel = levelGenerator.AddLevel();
        levelEnvironment.NextLevelEnvironment(subLevel, currentLevel.GetRowsCount() - 2);
        boxes = GetComponent<BoxSpawner>().SpawnBoxes(currentLevel, new Vector3(10.24f * subLevel, 0, -10), false);
        StartCoroutine(MoveCameraToNextLevel(oldBoxes, oldLevel));
    }

    private IEnumerator MoveCameraToNextLevel(Transform [] oldBoxes, Level oldLevel) {
        Transform transformCamera = Camera.main.transform;
        float startPositionX = transformCamera.localPosition.x;
        float endPositionX = 10.24f * subLevel;
        float time = 0;

        yield return new WaitForSeconds(0.4f);

        while (transformCamera.localPosition.x != endPositionX) {

            time += 0.06f;

            float _interpolatedValueX = Mathf.Lerp(startPositionX, endPositionX, time);
            transformCamera.localPosition = new Vector3(_interpolatedValueX, transformCamera.localPosition.y, transformCamera.localPosition.z);

            yield return new WaitForFixedUpdate();
        }

        levelEnvironment.UpdateObjectsPosition(subLevel);

        foreach (Transform box in oldBoxes) {
            Destroy(box.gameObject);
        }
        oldLevel.DestroyConveyors();
    }

    private void BlockBoxes(bool value) {
        for (int i = 0; i < boxes.Length; i++) {
            boxes[i].GetComponent<Box>().CanBeSelected = !value;
        }
    }

    private bool IsLevelOver() {
        for (int i = 0; i < boxes.Length; i++) {
            if (!boxes[i].GetComponent<Box>().IsEmpty()) {
                return false;
            }
        }
        return true;
    }

    private IEnumerator CloseBoxes(Box firstBox, Box secondBox) {
        yield return new WaitForSeconds(1f);
        firstBox.CloseBox();
        secondBox.CloseBox();
        firstBoxId = -1;
        secondBoxId = -1;

        if (GameMode == 3 && !isPlayerTurn && !currentLevel.IsCycle()) {
            DuelOpponentMove();
        }
    }

    private IEnumerator MoveBoxesOnConveyor() {
        yield return new WaitForSeconds(1f);
        for (int r = 0; r < currentLevel.GetCountOfCycles(); r++) {
            PlayConveyorSound();
            int[] movingBoxesId = currentLevel.GetBoxesIdForCycle(r);

            Vector3[] startBoxesPositions = new Vector3[movingBoxesId.Length];
            for (int i = 0; i < startBoxesPositions.Length; i++) {
                startBoxesPositions[i] = boxes[movingBoxesId[i]].localPosition;
            }
            float time = 0;

            if(GameMode == 3) {
                duelOpponent.UpdateStorageTimeInCycle(movingBoxesId);
            }

            currentLevel.MoveConveyorsInCycle(r);
            while (boxes[movingBoxesId[0]].localPosition != startBoxesPositions[1]) {
                for (int i = 0; i < movingBoxesId.Length; i++) {

                    time += 0.02f * (4f / movingBoxesId.Length);
                    int nextBoxNumber = i + 1;
                    if (nextBoxNumber > movingBoxesId.Length - 1) {
                        nextBoxNumber = 0;
                    }
                    float _interpolatedValueX = Mathf.Lerp(startBoxesPositions[i].x, startBoxesPositions[nextBoxNumber].x, time);
                    float _interpolatedValueY = Mathf.Lerp(startBoxesPositions[i].y, startBoxesPositions[nextBoxNumber].y, time);

                    boxes[movingBoxesId[i]].localPosition = new Vector3(_interpolatedValueX, _interpolatedValueY, startBoxesPositions[i].z);
                }
                yield return new WaitForFixedUpdate();
            }
            currentLevel.OffsetBoxesIdForCycle(r);
            currentLevel.StopConveyorsInCycle(r);
            StopConveyorSound();
        }

        if (!gameScore.Pause && isPlayerTurn) {
            BlockBoxes(false);
        }

        if (GameMode == 3 && !isPlayerTurn) {
            DuelOpponentMove();
        }
    }

    private void CheckAchievements() {
        switch(GameMode) {
            case 1:
                if (LevelNumber == 30) {
                    Social.ReportProgress(GPGSIds.achievement_stubborn_trainee, 100f, null);
                }
                bool isAllLevel3Stars = true;
                for (int i = 0; i < DataStorage.StarsForLevel.Length; i++) {
                    if (DataStorage.StarsForLevel[i] != 3) {
                        isAllLevel3Stars = false;
                    }
                }
                if (isAllLevel3Stars) {
                    Social.ReportProgress(GPGSIds.achievement_promising_trainee, 100f, null);
                }
                PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_the_harder_you_train, 1, null);
                break;
            case 2:
                PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_the_harder_you_train, 1, null);
                break;
            case 3:
                if (gameScore.PlayerDuelScore > gameScore.OpponentDuelScore) {
                    PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_consummate_worker, 1, null);
                }
                PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_the_harder_you_train, 1, null);
                break;
            case 4:
                if (gameScore.ScoreValue >= 20 && gameScore.ScoreValue > DataStorage.TimeAttackScore) {
                    Social.ReportProgress(GPGSIds.achievement_attentive_manager, 100f, null);
                }
                if (gameScore.ScoreValue >= 50 && gameScore.ScoreValue > DataStorage.TimeAttackScore) {
                    Social.ReportProgress(GPGSIds.achievement_do_you_want_a_promotion, 100f, null);
                }
                if (gameScore.ScoreValue >= 100 && gameScore.ScoreValue > DataStorage.TimeAttackScore) {
                    Social.ReportProgress(GPGSIds.achievement_ruthless_management, 100f, null);
                }
                if (DataStorage.TimeAttackScore < gameScore.ScoreValue) {
                    DataStorage.SetTimeAttackBestScore(gameScore.ScoreValue);
                    Social.ReportScore(gameScore.ScoreValue, GPGSIds.leaderboard_time_attack_score, null);
                }
                break;
        }
        PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_the_better_youll_fight, pairsCount, null);
    }

    public void PlayConveyorSound() {
        if (DataStorage.Sound) {
            conveyorAudioSource.Play();
        }
    }

    public void StopConveyorSound() {
        if (DataStorage.Sound) {
            conveyorAudioSource.Stop();
        }
    }

    public void PlayClickSound() {
        if (DataStorage.Sound) {
            audioSource.PlayOneShot(clickSound, 1f);
        }
    }

    public void PlayPairSound() {
        if (DataStorage.Sound) {
            audioSource.PlayOneShot(pairSound, 1f);
        }
    }

    private IEnumerator LoadDuelOpponent() {
        AssetReference duelOpponentReference = new AssetReference("Assets/Prefabs/Duel Opponent.prefab");
        AsyncOperationHandle duelOpponentOperationHandle = duelOpponentReference.LoadAssetAsync<GameObject>();
        yield return duelOpponentOperationHandle;
        duelOpponent = Instantiate((GameObject)duelOpponentOperationHandle.Result, new Vector3(0, 0, -10), Quaternion.identity).GetComponent<DuelOpponent>();
        duelOpponent.InitializeOpponentMemory(boxes.Length);
        Addressables.Release(duelOpponentOperationHandle);
    }
}
