using UnityEngine;
using UnityEngine.EventSystems;

public class BoxLevel : MonoBehaviour, IPointerDownHandler {

    public int level;
    public Sprite openBox;

    public int countOfRows;
    public int boxesOnRow;
    public int[] startCycleBoxId;
    public Vector2Int[] cyclesSize;
    public int topLevelMoves;
    public float topLevelTime;

    public SpriteRenderer levelCat;
    public SpriteRenderer starTablet;
    public GameObject ajarBox;
    public GameObject lockTape;

    public void ReadyToPlayBox() {
        ajarBox.SetActive(true);
    }

    public void CompleteBox(Sprite tablet, Sprite cat) {
        GetComponent<SpriteRenderer>().sprite = openBox;
        levelCat.gameObject.SetActive(true);
        levelCat.sprite = cat;
        starTablet.sprite = tablet;
        starTablet.gameObject.SetActive(true);
    }

    public void LockBox() {
        lockTape.SetActive(true);
    }

    public void OnPointerDown(PointerEventData eventData) {
        SetLevelParameters();
        FindObjectOfType<SelectLevel>().OpenLevel(level);
    }

    public void SetLevelParameters() {
        CurrentLevelParameters.LevelNumber = level;
        CurrentLevelParameters.CountOfRows = countOfRows;
        CurrentLevelParameters.BoxesOnRow = boxesOnRow;
        CurrentLevelParameters.StartCycleBoxId = startCycleBoxId;
        CurrentLevelParameters.CyclesSize = cyclesSize;
        CurrentLevelParameters.TopLevelMoves = topLevelMoves;
        CurrentLevelParameters.TopLevelTime = topLevelTime;
    }
}
