using UnityEngine;

public class LevelEnvironment : MonoBehaviour {

    public LevelBackgroundKit mainLevelBackground;
    private LevelBackgroundKit copyLevelBackground;
    private float nextLevelOffset = 10.24f;

    public void InitializeLevelEnvironment(int gameMode, int countOfConveyorsRows) {
        if (gameMode == 4 || gameMode == 0) {
            Vector3 spawnPosition = mainLevelBackground.GetComponent<Transform>().localPosition + new Vector3(nextLevelOffset, 0, 0);
            copyLevelBackground = Instantiate(mainLevelBackground, spawnPosition, Quaternion.identity);
        }
        mainLevelBackground.SetActiveTapes(countOfConveyorsRows, false);
    }

    public void NextLevelEnvironment(int subLevel, int countOfConveyorsRows) {
        if (subLevel % 2 == 0) {
            mainLevelBackground.SetActiveTapes(countOfConveyorsRows, false);
        }
        else {
            copyLevelBackground.SetActiveTapes(countOfConveyorsRows, false);
        }
    }

    public void UpdateObjectsPosition(int subLevel) {
        float newPositionX = nextLevelOffset * (subLevel + 1);
        if (subLevel % 2 == 1) {
            mainLevelBackground.SetNextLevelObjectsPosition(newPositionX);
        }
        else {
            copyLevelBackground.SetNextLevelObjectsPosition(newPositionX);
        }
    }
}
