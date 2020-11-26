using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevel : MonoBehaviour {

    public static BoxLevel[] AreaLevels { get; set; }
    public BoxLevel[] BoxLevels;
    public Sprite[] starTablets;
    public Sprite[] levelCatBox;
    public Button nextPageButton;
    public Button previousPageButton;
    public Transform page1;
    public Transform page2;
    public AudioClip clickSound;
    public AudioSource conveyorAudioSource;
    private int currentPage = 1;
    private int currentLevel = -1;
    private AudioSource audioSource;
    public MenuConveyors conveyors;

    private void Start() {
        conveyors = GetComponent<MenuConveyors>();
        audioSource = GetComponent<AudioSource>();
        if (AreaLevels == null) {
            AreaLevels = new BoxLevel[BoxLevels.Length];
            for (int i = 0; i < AreaLevels.Length; i++) {
                AreaLevels[i] = BoxLevels[i];
            }
        }
        
        for (int i = 0; i < BoxLevels.Length; i++) {
            int levelStars = DataStorage.GetCountOfStars(i + 1);

            if (levelStars != 0) {
                BoxLevels[i].CompleteBox(starTablets[levelStars - 1], levelCatBox[levelStars - 1]);
            }
            else {
                if (currentLevel == -1) {
                    currentLevel = i + 1;
                    BoxLevels[i].ReadyToPlayBox();
                }
                else {
                    BoxLevels[i].LockBox();
                }
            }
        }

        if (currentLevel == -1) {
            currentLevel = BoxLevels.Length;
        }
        else if (currentLevel >= 16 && currentLevel <= 30) {
            currentPage = 2;
            page1.localPosition = new Vector3(-7.2f, 0, -1);
            page2.localPosition = new Vector3(0, 0, -1);
        }
    }

    public void NextPage() {
        if (currentPage == 2) {
            return;
        }
        PlayClickSound();
        PlayConveyorSound();
        conveyors.ShiftPagesToLeft();
        SetPageButtonsInteractable(false);
        currentPage++;
        StartCoroutine(UnblockButtons());
    }

    public void PreviousPage() {
        if (currentPage == 1) {
            return;
        }
        PlayClickSound();
        PlayConveyorSound();
        conveyors.ShiftPagesToRight();
        SetPageButtonsInteractable(false);
        currentPage--;
        StartCoroutine(UnblockButtons());
    }

    private IEnumerator UnblockButtons() {
        yield return new WaitForSeconds(0.5f);
        SetPageButtonsInteractable(true);
    }

    private void SetPageButtonsInteractable(bool value) {
        nextPageButton.interactable = value;
        previousPageButton.interactable = value;
    }

    public void ReturnToAreas() {
        PlayClickSound();
        Transition.GetInstance().EndScene("Areas");
    }

    public void OpenLevel(int level) {
        if (level > currentLevel) {
            return;
        }
        PlayClickSound();
        if (level >= 1 && level <= 15 && currentPage == 1 || level >= 16 && level <= 30 && currentPage == 2) {
            Game.LevelNumber = level;
            Game.GameMode = 1;
            Transition.GetInstance().EndScene("Game");
        }
    }

    public void PlayClickSound() {
        if (DataStorage.Sound) {
            audioSource.PlayOneShot(clickSound, 1f);
        }
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
}
