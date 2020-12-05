using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour {

    public Transition transition;
    public Animator pauseAnimator;
    public Text pauseText;
    private bool closing = false;

    public void PauseGame() {
        closing = false;
        pauseText.text = LanguageTitles.GetInstance().pauseTitle;
        FindObjectOfType<Game>().PauseGame();
        SetActivePause(true);
        pauseAnimator.SetTrigger("Pause Background");
    }

    public void OpenMenuScene() {
        if (Game.GameMode != 4) {
            FindObjectOfType<AdManager>().IsInGame = false;
        }
        TasksData.SaveTasksData();
        transition.EndScene("Menu");
    }

    public void RestartLevel() {
        if (Game.GameMode != 4) {
            FindObjectOfType<AdManager>().IsInGame = false;
        }
        transition.EndScene("Game");
    }

    public void ShowPauseButtons() {
        if (!closing) {
            if (Game.GameMode == 1) {
                pauseAnimator.SetTrigger("Buttons 1");
            }
            else {
                pauseAnimator.SetTrigger("Buttons 2");
            }
        }
    }

    public void ClosePauseButton() {
        if (closing) {
            pauseAnimator.SetTrigger("Close Buttons");
        }
    }

    private void SetActivePause(bool value) {
        gameObject.SetActive(value);
    }

    public void DiactivatePause() {
        if (closing) {
            SetActivePause(false);
        }
    }

    public void ContinueGame() {
        closing = true;
        pauseAnimator.SetTrigger("Close Background");
        FindObjectOfType<Game>().ContinueGame();
    }
}
