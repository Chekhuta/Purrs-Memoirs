using UnityEngine;
using UnityEngine.UI;

public class QuickGamePanel : MonoBehaviour {

    public static Vector2Int[] QuickGameSizes { get; set; }
    public static int[] QuickGameCycleTypes { get; set; }

    public GameObject panel;
    public Transition transition;
    public Button playButton;
    public Image[] sizeButtons;
    public Image[] cycleButtons;
    public Sprite buttonSelected;
    public Sprite buttonPlaceholder;
    public Animator quickGameAnimator;
    private bool closingPanel = false;
    private bool blockButtons = false;
    private Sounds sounds;

    private bool[] levelSizeSelected = {
        false, false, false,
        false, false, false,
        false, false, false,
        false, false, false
    };
    private bool[] levelCycleSelected = { false, false, false };

    private Vector2Int[] levelSize = {
        new Vector2Int(2, 2), new Vector2Int(2, 3), new Vector2Int(5, 2),
        new Vector2Int(3, 4), new Vector2Int(6, 3), new Vector2Int(4, 4),
        new Vector2Int(5, 4), new Vector2Int(6, 4), new Vector2Int(4, 7),
        new Vector2Int(5, 6), new Vector2Int(6, 6), new Vector2Int(6, 7)
    };
    private int[] levelCycleType = { 0, 1, 2 };

    public void SizeButtonAction(int buttonIndex) {
        if (blockButtons) {
            return;
        }
        PlayLightClickSound();
        if (levelSizeSelected[buttonIndex]) {
            levelSizeSelected[buttonIndex] = false;
            sizeButtons[buttonIndex].sprite = buttonPlaceholder;
            if (!IsSelectedButtons(levelSizeSelected) || !IsSelectedButtons(levelCycleSelected)) {
                playButton.interactable = false;
            }
        }
        else {
            levelSizeSelected[buttonIndex] = true;
            sizeButtons[buttonIndex].sprite = buttonSelected;
            if (IsSelectedButtons(levelSizeSelected) && IsSelectedButtons(levelCycleSelected)) {
                playButton.interactable = true;
            }
        }
    }

    public void CycleButtonAction(int buttonIndex) {
        if (blockButtons) {
            return;
        }
        PlayLightClickSound();
        if (levelCycleSelected[buttonIndex]) {
            levelCycleSelected[buttonIndex] = false;
            cycleButtons[buttonIndex].sprite = buttonPlaceholder;
            if (!IsSelectedButtons(levelSizeSelected) || !IsSelectedButtons(levelCycleSelected)) {
                playButton.interactable = false;
            }
        }
        else {
            levelCycleSelected[buttonIndex] = true;
            cycleButtons[buttonIndex].sprite = buttonSelected;
            if (IsSelectedButtons(levelSizeSelected) && IsSelectedButtons(levelCycleSelected)) {
                playButton.interactable = true;
            }
        }
    }

    public void OpenQuickGamePanel() {
        closingPanel = false;
        panel.SetActive(true);
        gameObject.SetActive(true);
        quickGameAnimator.SetTrigger("Show");
    }

    public void CloseQuickGamePanel() {
        if (blockButtons) {
            return;
        }
        PlayClickSound();
        FindObjectOfType<OpponentAnimation>().SetCanCount(true);
        closingPanel = true;
        quickGameAnimator.SetTrigger("Close");
    }

    public void DeactivatePanel() {
        if (closingPanel) {
            FindObjectOfType<Menu>().SetActiveButtons(true);
            panel.SetActive(false);
        }
    }

    public void StartGame() {
        if (blockButtons) {
            return;
        }
        blockButtons = true;
        PlayClickSound();
        int len = CountOfTrueButtons(levelSizeSelected);
        QuickGameSizes = new Vector2Int[len];
        int index = 0;
        for (int i = 0; i < levelSizeSelected.Length; i++) {
            if (levelSizeSelected[i]) {
                QuickGameSizes[index] = levelSize[i];
                index++;
            }
        }

        len = CountOfTrueButtons(levelCycleSelected);
        QuickGameCycleTypes = new int[len];
        index = 0;
        for (int i = 0; i < levelCycleSelected.Length; i++) {
            if (levelCycleSelected[i]) {
                QuickGameCycleTypes[index] = levelCycleType[i];
                index++;
            }
        }

        Game.GameMode = 2;
        transition.EndScene("Game");
    }

    private int CountOfTrueButtons(bool[] array) {
        int len = 0;
        for (int i = 0; i < array.Length; i++) {
            if (array[i]) {
                len++;
            }
        }
        return len;
    }

    private bool IsSelectedButtons(bool[] array) {
        for (int i = 0; i < array.Length; i++) {
            if (array[i]) {
                return true;
            }
        }
        return false;
    }

    private void PlayLightClickSound() {
        Sounds.GetInstance().PlayLightClickSound();
    }

    private void PlayClickSound() {
        Sounds.GetInstance().PlayClickSound();
    }

}
