using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TasksPanel : MonoBehaviour {

    public Sprite[] catsIcons;
    public Sprite timeAttackIcon;
    public Sprite duelIcon;
    public Sprite tutorialIcon;
    public Sprite pairsIcon;
    public Sprite fieldIcon;
    public Image fadeBackground;
    public RectTransform panelRect;
    public Button closeButton;

    public Text tasksHeaderText;
    public RectTransform[] tasksRect;
    public Text[] tasksText;
    public Image[] tasksIcon;
    public RectTransform[] tasksCheckMark;
    public Button[] rewardButtons;

    public GameObject coin;
    public RectTransform coinRectParent;
    public CoinField coinField;
    private int closeButtonWait = 0;

    public void UpdateTasks() {
        for (int i = 0; i < tasksText.Length; i++) {
            UpdateTask(i);
        }
        tasksHeaderText.text = LanguageTitles.GetInstance().taskTitle;
    }

    private void UpdateTask(int taskIndex) {
        tasksText[taskIndex].text = TasksData.Tasks[taskIndex].GetDescription();
        SetTaskIcon(taskIndex);
        rewardButtons[taskIndex].interactable = TasksData.Tasks[taskIndex].IsTaskCompleted();
        tasksCheckMark[taskIndex].gameObject.SetActive(TasksData.Tasks[taskIndex].IsTaskCompleted());
        rewardButtons[taskIndex].GetComponentInChildren<Text>().text = TasksData.Tasks[taskIndex].GetProgress();
    }

    public void GetReward(int taskIndex) {
        closeButton.interactable = false;
        closeButtonWait++;
        PlayClickSound();
        int coinsCount = Mathf.Clamp(TasksData.Tasks[taskIndex].GetReward() / 10, 5, 10);
        RectTransform[] rewardCoins = new RectTransform[coinsCount];
            
        for (int i = 0; i < rewardCoins.Length; i++) {
            rewardCoins[i] = Instantiate(coin, coinRectParent).GetComponent<RectTransform>();
            rewardCoins[i].anchoredPosition = new Vector2(64, 156 - (taskIndex * 176));
        }

        rewardButtons[taskIndex].GetComponentInChildren<Text>().text = "+" + TasksData.Tasks[taskIndex].GetReward();
        StartCoroutine(MoveCoinsToCoinField(rewardCoins, TasksData.Tasks[taskIndex].GetReward(), taskIndex));

        TasksData.Coins += TasksData.Tasks[taskIndex].GetReward();
        TasksData.GiveNewTask(taskIndex);
        rewardButtons[taskIndex].interactable = false;
        if (!TasksData.IsCompletedTasks()) {
            FindObjectOfType<Menu>().SetEnabledTaskSign(false);
        }
    }

    private IEnumerator MoveCoinsToCoinField(RectTransform[] coinsRect, int reward, int taskIndex) {
        Vector3[] coinsPathPositions = new Vector3[coinsRect.Length];
        Vector3[] startPositions = new Vector3[coinsRect.Length];
        for(int i = 0; i < coinsRect.Length; i++) {
            float x = 0;
            if ((x = Random.Range(-90, 218)) % 2 != 0) {
                x += 1;
            }
            float yOffset = 0;
            if ((yOffset = Random.Range(0, 60)) % 2 != 0) {
                yOffset += 1;
            }
            float y = coinsRect[i].localPosition.y + yOffset;
            coinsPathPositions[i] = new Vector3(x, y, 0);
            startPositions[i] = coinsRect[i].localPosition;
        }

        float movingTime = 0;
        while (coinsRect[0].localPosition != coinsPathPositions[0]) {
            movingTime += 0.06f;
            for (int i = 0; i < coinsRect.Length; i++) {
                float _interpolatedScaleValueX = Mathf.Lerp(coinsRect[i].localPosition.x, coinsPathPositions[i].x, movingTime);
                float _interpolatedScaleValueY = Mathf.Lerp(coinsRect[i].localPosition.y, coinsPathPositions[i].y, movingTime);
                coinsRect[i].localPosition = new Vector3(_interpolatedScaleValueX, _interpolatedScaleValueY, 0);
            }
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < coinsRect.Length; i++) {
            coinsRect[i].SetParent(coinField.GetCoinRectParent());
        }
        Vector3 coinFieldPosition = coinField.GetCoinImagePosition();
        movingTime = 0;
        while (coinsRect[0].localPosition != coinFieldPosition) {
            movingTime += 0.06f;
            for (int i = 0; i < coinsRect.Length; i++) {
                float _interpolatedScaleValueX = Mathf.Lerp(coinsRect[i].localPosition.x, coinFieldPosition.x, movingTime);
                float _interpolatedScaleValueY = Mathf.Lerp(coinsRect[i].localPosition.y, coinFieldPosition.y, movingTime);
                coinsRect[i].localPosition = new Vector3(_interpolatedScaleValueX, _interpolatedScaleValueY, 0);
            }
            yield return new WaitForFixedUpdate();
        }
        for (int i = 0; i < coinsRect.Length; i++) {
            Destroy(coinsRect[i].gameObject);
        }
        PlayCoinSound();
        coinField.ShowBonusText(reward);
        coinField.UpdateCoinField();

        yield return new WaitForSeconds(0.6f);
        movingTime = 0;
        while (tasksRect[taskIndex].localScale.y != 0) {
            movingTime += 0.25f;
            float _interpolatedScaleValueY = Mathf.Lerp(1, 0, movingTime);
            tasksRect[taskIndex].localScale = new Vector3(tasksRect[taskIndex].localScale.x, _interpolatedScaleValueY, 1);
            yield return new WaitForFixedUpdate();
        }
        UpdateTask(taskIndex);
        movingTime = 0;
        while (tasksRect[taskIndex].localScale.y != 1) {
            movingTime += 0.25f;
            float _interpolatedScaleValueY = Mathf.Lerp(0, 1, movingTime);
            tasksRect[taskIndex].localScale = new Vector3(tasksRect[taskIndex].localScale.x, _interpolatedScaleValueY, 1);
            yield return new WaitForFixedUpdate();
        }
        closeButtonWait--;
        if (closeButtonWait == 0) {
            closeButton.interactable = true;
        }
    }

    private void SetTaskIcon(int taskIndex) {
        switch(TasksData.Tasks[taskIndex].GetTaskTypeName()) {
            case TaskTypeName.TutorialTask:
                tasksIcon[taskIndex].sprite = tutorialIcon;
                break;
            case TaskTypeName.FindCatTask:
                tasksIcon[taskIndex].sprite = catsIcons[((FindCatTask)TasksData.Tasks[taskIndex]).GetCatId()];
                break;
            case TaskTypeName.FindPairsTask:
                tasksIcon[taskIndex].sprite = pairsIcon;
                break;
            case TaskTypeName.DuelTask:
                tasksIcon[taskIndex].sprite = duelIcon;
                break;
            case TaskTypeName.TimeAttackTask:
                tasksIcon[taskIndex].sprite = timeAttackIcon;
                break;
            case TaskTypeName.FieldTask:
                tasksIcon[taskIndex].sprite = fieldIcon;
                break;
        }
    }

    public void ShowPanel() {
        UpdateTasks();
        gameObject.SetActive(true);
        panelRect.gameObject.SetActive(true);
        panelRect.localScale = new Vector3(0, 0, 1);
        StartCoroutine(ShowAnimation());
    }

    public void ClosePanel() {
        PlayClickSound();
        FindObjectOfType<OpponentAnimation>().SetCanCount(true);
        StartCoroutine(CloseAnimation());
    }

    private IEnumerator ShowAnimation() {
        fadeBackground.gameObject.SetActive(true);
        float movingTime = 0;
        Color fadeColor = fadeBackground.color;

        while (panelRect.localScale.y != 1) {
            movingTime += 0.12f;
            float _interpolatedScaleValue = Mathf.Lerp(0, 1, movingTime);
            fadeColor.a = Mathf.Lerp(0, 1, movingTime);
            panelRect.localScale = new Vector3(_interpolatedScaleValue, _interpolatedScaleValue, 1);
            fadeBackground.color = fadeColor;
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator CloseAnimation() {
        float movingTime = 0;
        Color fadeColor = fadeBackground.color;

        while (panelRect.localScale.y != 0) {
            movingTime += 0.12f;
            float _interpolatedScaleValue = Mathf.Lerp(1, 0, movingTime);
            fadeColor.a = Mathf.Lerp(1, 0, movingTime); ;
            panelRect.localScale = new Vector3(_interpolatedScaleValue, _interpolatedScaleValue, 1);
            fadeBackground.color = fadeColor;
            yield return new WaitForFixedUpdate();
        }
        FindObjectOfType<Menu>().SetActiveButtons(true);
        panelRect.gameObject.SetActive(false);
        fadeBackground.gameObject.SetActive(false);
    }

    private void PlayClickSound() {
        Sounds.GetInstance().PlayClickSound();
    }

    private void PlayCoinSound() {
        Sounds.GetInstance().PlayCoinSound();
    }
}
