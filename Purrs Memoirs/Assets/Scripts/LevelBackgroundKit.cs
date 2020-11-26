using UnityEngine;

public class LevelBackgroundKit : MonoBehaviour {

    public GameObject[] tapes;
    public Transform background;

    public void SetNextLevelObjectsPosition(float newPositionX) {
        background.localPosition = new Vector3(newPositionX, background.localPosition.y, background.localPosition.z);
        SetActiveTapes(tapes.Length, true);
    }

    public void SetActiveTapes(int countOfConveyorsRows, bool isActive) {
        for (int i = 0; i < countOfConveyorsRows; i++) {
            tapes[i].SetActive(isActive);
        }
    }
}
