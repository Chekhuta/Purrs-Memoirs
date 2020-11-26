using UnityEngine;
using System.Collections.Generic;

public class BoxContentGenerator : MonoBehaviour {

    public GameObject[] catFrames;
    private int[] valuesForContent;

    public void InstantiateCatFrames(Transform box, int boxIndex) {
        GameObject catFramesObject = Instantiate(catFrames[valuesForContent[boxIndex]], box);
        box.GetComponent<Box>().InitializeBoxContent(boxIndex, valuesForContent[boxIndex], catFramesObject);
    }

    public void InitValuesForContent(int countOfBoxes) {

        List<int> catFramesIndex = new List<int>();

        for (int i = 0; i < catFrames.Length; i++) {
            catFramesIndex.Add(i);
        }

        valuesForContent = new int[countOfBoxes];

        for (int i = 0; i < countOfBoxes / 2; i++) {
            int randomIndex = Random.Range(0, catFramesIndex.Count);
            int catIndex = catFramesIndex[randomIndex];
            catFramesIndex.RemoveAt(randomIndex);
            valuesForContent[i * 2] = catIndex;
            valuesForContent[i * 2 + 1] = catIndex;
        }

        for (int i = 0; i < countOfBoxes - 1; i++) {
            int j = Random.Range(i + 1, countOfBoxes);
            int temp = valuesForContent[i];
            valuesForContent[i] = valuesForContent[j];
            valuesForContent[j] = temp;
        }
    }
}
