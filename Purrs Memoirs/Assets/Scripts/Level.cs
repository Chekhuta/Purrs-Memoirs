using UnityEngine;

public class Level : MonoBehaviour {

    public int levelNumber;
    public int rowsCount;
    public int[] placesOnRow;

    public Vector3[] boxesPosition;
    public ConveyorLevelAnimation[] conveyorsAnimation;
    public int[][] conveyorsInCycle;
    public Cycle[] cycles;
    public GameObject[] spawners;

    public void InitLevel(int level, int rows, int[] places, Vector3[] positions, ConveyorLevelAnimation[] conveyors, Cycle[] cycleConveyors, GameObject[] boxSpawners) {
        levelNumber = level;
        rowsCount = rows;
        placesOnRow = places;
        boxesPosition = positions;
        conveyorsAnimation = conveyors;
        cycles = cycleConveyors;
        spawners = boxSpawners;
    }

    public int GetRowsCount() {
        return rowsCount;
    }

    public int GetNumberOfBoxesOnRow(int row) {
        return placesOnRow[row];
    }

    public Vector3[] GetBoxesPosition() {
        return boxesPosition;
    }

    public int GetCountOfBoxes() {
        return boxesPosition.Length;
    }

    public int GetMaxBoxCountOnRow() {
        int max = placesOnRow[0];
        for (int i = 1; i < placesOnRow.Length; i++) {
            if (placesOnRow[i] > max) {
                max = placesOnRow[i];
            }
        }
        return max;
    }

    public int[] GetBoxesIdForCycle(int cycleIndex) {
        if (cycles[cycleIndex].BoxesIdForCycle != null) {
            return cycles[cycleIndex].BoxesIdForCycle;
        }
        else {
            return new int[0];
        }
    }

    public bool IsCycle() {
        if (cycles != null && cycles.Length != 0) {
            return cycles[0].BoxesIdForCycle.Length > 0;
        }
        else {
            return false;
        }
    }

    public void MoveConveyorsAtStart() {
        foreach (ConveyorLevelAnimation c in conveyorsAnimation) {
            if (c.ConveyorType != 4 && c.ConveyorType != 3) {
                c.MoveLeft(0);
            }
            else if (c.ConveyorType == 3) {
                c.MoveLeftAngel();
            }
        }
    }

    public void StopConveyors() {
        foreach (ConveyorLevelAnimation c in conveyorsAnimation) {
            c.StopMoving();
        }
    }

    public void MoveConveyorsInCycle(int cycleIndex) {
        int direction = 1;
        for (int i = 0; i < cycles[cycleIndex].ConveyorsInCycle.Length; i++) {
            int index = cycles[cycleIndex].ConveyorsInCycle[i];
            if (direction == 1) {
                conveyorsAnimation[index].MoveRight(cycleIndex);
            }
            else {
                conveyorsAnimation[index].MoveLeft(cycleIndex);
            }
            if (index == cycles[cycleIndex].ChangeDirectoryConveyorId) {
                direction = -1;
            }
        }
    }

    public void StopConveyorsInCycle(int cycleIndex) {
        for (int i = 0; i < cycles[cycleIndex].ConveyorsInCycle.Length; i++) {
            conveyorsAnimation[cycles[cycleIndex].ConveyorsInCycle[i]].StopMoving();
        }
    }

    public void OffsetBoxesIdForCycle(int cycleIndex) {
        int[] oldIds = new int[cycles[cycleIndex].BoxesIdForCycle.Length];
        cycles[cycleIndex].BoxesIdForCycle.CopyTo(oldIds, 0);
        for (int i = cycles[cycleIndex].BoxesIdForCycle.Length - 1; i > 0; i--) {
            int temp = cycles[cycleIndex].BoxesIdForCycle[i - 1];
            cycles[cycleIndex].BoxesIdForCycle[i - 1] = cycles[cycleIndex].BoxesIdForCycle[i];
            cycles[cycleIndex].BoxesIdForCycle[i] = temp;
        }

        for (int i = 0; i < cycles.Length; i++) {
            if (i != cycleIndex) {
                for (int j = 0; j < cycles[i].BoxesIdForCycle.Length; j++) {
                    for (int k = 0; k < oldIds.Length; k++) {
                        if (cycles[i].BoxesIdForCycle[j] == oldIds[k]) {
                            cycles[i].BoxesIdForCycle[j] = cycles[cycleIndex].BoxesIdForCycle[k];
                            break;
                        }
                    }
                }
            }
        }
    }

    public int GetCountOfCycles() {
        return cycles.Length;
    }

    public void DestroyConveyors() {
        foreach (ConveyorLevelAnimation c in conveyorsAnimation) {
            Destroy(c.gameObject);
        }
        Destroy(this);
    }
}
