using System.Collections;
using UnityEngine;

public class BoxSpawner : MonoBehaviour {

    public GameObject boxPrefab;
    private BoxContentGenerator contentGenerator;
    private Level level;
    private float moveSpeed = 2;

    public Transform[] SpawnBoxes(Level currentLevel, Vector3 spawnPosition, bool moveToZero) {
        level = currentLevel;
        contentGenerator = GetComponent<BoxContentGenerator>();
        GetComponent<BoxContentGenerator>().InitValuesForContent(currentLevel.GetCountOfBoxes());
        Transform[] boxes = new Transform[currentLevel.GetCountOfBoxes()];
        Vector3[] boxesPosition = currentLevel.GetBoxesPosition();
        int repeatNumber = currentLevel.GetMaxBoxCountOnRow();

        for (int i = 0; i < repeatNumber; i++) {
            for (int j = 0; j < currentLevel.GetRowsCount(); j++) {
                if (i < currentLevel.GetNumberOfBoxesOnRow(j)) {
                    int boxIndex = BoxIndex(currentLevel, i, j);
                    Vector3 offset = new Vector3(boxesPosition[boxIndex].x, boxesPosition[boxIndex].y, 0);
                    boxes[boxIndex] = Instantiate(boxPrefab, spawnPosition + offset, Quaternion.identity).GetComponent<Transform>();
                    contentGenerator.InstantiateCatFrames(boxes[boxIndex], boxIndex);
                }
            }
        }
        if (moveToZero) {
            StartCoroutine(MoveBoxFromSpawn(boxes, boxesPosition, boxes[0].localPosition.x - boxesPosition[0].x));
            Game.GetInstance().PlayConveyorSound();
            currentLevel.MoveConveyorsAtStart();
        }
        return boxes;
    }

    private int BoxIndex(Level currentLevel, int i, int j) {
        int index = 0;
        j--;
        for (int k = j; k >= 0; k--) {
            index += currentLevel.GetNumberOfBoxesOnRow(k);
        }
        return index + i;
    }

    private IEnumerator MoveBoxFromSpawn(Transform[] boxes, Vector3[] boxesPosition, float offsetX) {
        float time = 0;
        float newOffsetX = offsetX;
        while (newOffsetX != 0) {

            time += moveSpeed * 0.02f;

            newOffsetX = Mathf.Lerp(offsetX, 0, time);
            Vector3 offset = new Vector3(newOffsetX, 0, 0);
            for (int i = 0; i < boxes.Length; i++) {
                boxes[i].localPosition = boxesPosition[i] + offset;
            }

            yield return new WaitForFixedUpdate();
        }
        level.StopConveyors();
        Game.GetInstance().StopConveyorSound();
        StartCoroutine(MoveSpawnerToRight());
    }

    private IEnumerator MoveSpawnerToRight() {

        GameObject[] spawnersGameObject = level.spawners;
        Transform[] spawner = new Transform[spawnersGameObject.Length];
        for (int i = 0; i < spawnersGameObject.Length; i++) {
            spawner[i] = spawnersGameObject[i].GetComponent<Transform>();
        }

        float time = 0;
        float startPositionX = spawner[0].localPosition.x;
        float endPositionX;
        if (Game.GameMode == 4 || Game.GameMode == 0) {
            endPositionX = spawner[0].localPosition.x + 0.46f * 6;
        }
        else {
            endPositionX = spawner[0].localPosition.x + 0.46f;
        }

        while (spawner[0].localPosition.x != endPositionX) {
            time += moveSpeed * 0.02f;
            float _interpolatedValueX = Mathf.Lerp(startPositionX, endPositionX, time);
            for (int i = 0; i < spawner.Length; i++) {
                spawner[i].localPosition = new Vector3(_interpolatedValueX, spawner[i].localPosition.y, spawner[i].localPosition.z);
            }
            yield return new WaitForFixedUpdate();
        }
        if (Game.GameMode == 4 || Game.GameMode == 0) {
            for (int i = 0; i < spawner.Length; i++) {
                Destroy(spawner[i].gameObject);
            }
        }
    }

}
