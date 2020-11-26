using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuelOpponent : MonoBehaviour {

    public Sprite[] opponentSprites;
    private Vector3 offset = new Vector3(0, 0.6f, 0);

    [System.Serializable]
    public class MemoryCell {
        public int BoxId;
        public int ValueId;
        public int StorageTime;

        public MemoryCell(int box) {
            BoxId = box;
            ValueId = -1;
            StorageTime = 0;
        }
    }

    private List<MemoryCell> opponentMemory;

    public void InitializeOpponentMemory(int boxCount) {
        opponentMemory = new List<MemoryCell>();

        for (int i = 0; i < boxCount; i++) {
            opponentMemory.Add(new MemoryCell(i));
        }
    }

    public int[] TurnBoxesId(Game game) {

        for (int i = 0; i < opponentMemory.Count - 1; i++) {
            if (opponentMemory[i].ValueId == -1) {
                continue;
            }
            for (int j = i + 1; j < opponentMemory.Count; j++) {
                if (opponentMemory[j].ValueId == -1) {
                    continue;
                }
                if (opponentMemory[i].ValueId == opponentMemory[j].ValueId) {
                    return new int[] { opponentMemory[i].BoxId, opponentMemory[j].BoxId };
                }
            }
        }

        List<MemoryCell> unknownValueBox = new List<MemoryCell>();

        foreach(MemoryCell m in opponentMemory) {
            if (m.ValueId == -1) {
                unknownValueBox.Add(m);
            }
        }

        if (Random.Range(0, 3) == 0) {
            int cellIndex1 = Random.Range(0, unknownValueBox.Count);
            int boxValue = game.GetBoxValue(unknownValueBox[cellIndex1].BoxId);
            foreach (MemoryCell m in opponentMemory) {
                if (m.ValueId == boxValue) {
                    int boxId1 = unknownValueBox[cellIndex1].BoxId;
                    unknownValueBox.Clear();
                    return new int[] { boxId1, m.BoxId };
                }
            }
        }

        if (unknownValueBox.Count >= 2) {
            int[] boxesIds = RandomBoxesIds(unknownValueBox);
            unknownValueBox.Clear();
            return boxesIds;
        }
        else {
            return RandomBoxesIds(opponentMemory);
        }
    }

    private int[] RandomBoxesIds(List<MemoryCell> currentList) {
        int cellIndex1 = Random.Range(0, currentList.Count);
        int cellIndex2 = Random.Range(0, currentList.Count);
        while (cellIndex1 == cellIndex2) {
            cellIndex2 = Random.Range(0, currentList.Count);
        }
        return new int[] { currentList[cellIndex1].BoxId, currentList[cellIndex2].BoxId };
    }

    public void AddToMemory(Box box1, Box box2) {
        foreach (MemoryCell m in opponentMemory) {
            if (m.BoxId == box1.BoxId) {
                m.ValueId = box1.ContentValue;
                m.StorageTime = 0;
            }
            if (m.BoxId == box2.BoxId) {
                m.ValueId = box2.ContentValue;
                m.StorageTime = 0;
            }
        }
        CheckStorageTime();
    }

    public void UpdateStorageTimeInCycle(int[] boxesIds) {

        List<MemoryCell> memoryCellsCycle = new List<MemoryCell>();

        foreach (MemoryCell m in opponentMemory) {
            if (m.ValueId == -1) {
                continue;
            }
            for (int i = 0; i < boxesIds.Length; i++) {
                if (m.BoxId == boxesIds[i]) {
                    memoryCellsCycle.Add(m);
                    break;
                }
            }
        }

        for (int i = 0, max = Mathf.CeilToInt(memoryCellsCycle.Count / 2); i < max; i++) {
            memoryCellsCycle.RemoveAt(Random.Range(0, memoryCellsCycle.Count));
        }

        foreach (MemoryCell m in memoryCellsCycle) {
            m.StorageTime++;
            if (m.StorageTime >= 4) {
                m.ValueId = -1;
                m.StorageTime = 0;
            }
        }
        memoryCellsCycle.Clear();
    }

    private void CheckStorageTime() {
        foreach (MemoryCell m in opponentMemory) {
            if (m.ValueId != -1) {
                m.StorageTime++;
                if (m.StorageTime >= 4) {
                    m.ValueId = -1;
                    m.StorageTime = 0;
                }
            }
        }
    }

    public void DeleteEmptyBoxes(int box1Id, int box2Id) {
        DeleteBox(box1Id);
        DeleteBox(box2Id);
    }

    private void DeleteBox(int boxId) {
        foreach (MemoryCell m in opponentMemory) {
            if (m.BoxId == boxId) {
                opponentMemory.Remove(m);
                return;
            }
        }
    }

    public IEnumerator OpenBoxes(Box box1, Box box2) {

        SpriteRenderer opponentRenderer = GetComponent<SpriteRenderer>();

        yield return StartCoroutine(OpenBoxByOpponent(0.5f, box1, opponentRenderer));
        yield return StartCoroutine(OpenBoxByOpponent(0.1f, box2, opponentRenderer));
    }

    private IEnumerator OpenBoxByOpponent(float delay, Box box, SpriteRenderer opponentRenderer) {

        yield return new WaitForSeconds(delay);
        transform.localPosition = box.transform.localPosition + offset;
        opponentRenderer.enabled = true;

        for (int i = 0; i < opponentSprites.Length; i++) {
            opponentRenderer.sprite = opponentSprites[i];
            yield return new WaitForSeconds(0.05f);
        }

        opponentRenderer.sprite = opponentSprites[1];
        yield return new WaitForSeconds(0.3f);

        opponentRenderer.sprite = opponentSprites[0];
        yield return new WaitForSeconds(0.05f);

        opponentRenderer.enabled = false;
        box.OpenBoxAnimated();
    }
}
