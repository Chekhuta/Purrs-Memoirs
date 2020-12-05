using System.Collections.Generic;
using UnityEngine;

public class ConveyorsFieldGenerator : MonoBehaviour {

    public GameObject angleConveyor;
    public GameObject verticalConveyor;
    public GameObject onePlaceConveyor;
    public GameObject threePlaceConveyor;
    public GameObject spawner;

    private Vector3[] boxesPosition;
    private ConveyorLevelAnimation[] conveyorsAnimation;

    private float[] conveyorsCoordinatesY = { 3.68f, 1.84f, 0f, -1.84f, -3.68f, -5.52f };
    private float spawnOffsetX = 0;
    private int[,] conveyorsPositions;
    private int[,] boxesId;

    private int[][,] cycle;
    private Vector2Int[] startCycleCoords;

    private int countOfRows;
    private int boxesOnRow;
    private int countOfCycles;
    private Cycle[] levelCycles;
    private GameObject[] spawners;

    public void InitConveyorsPositions(int gameMode, int levelNumber) {
        spawnOffsetX = 10.24f * levelNumber;

        switch(gameMode) {
            case 0:
                Tutorial.SetLevelParameters(levelNumber);
                countOfCycles = CurrentLevelParameters.StartCycleBoxId.Length;
                countOfRows = CurrentLevelParameters.CountOfRows;
                boxesOnRow = CurrentLevelParameters.BoxesOnRow;
                break;
            case 1:
                countOfCycles = CurrentLevelParameters.StartCycleBoxId.Length;
                countOfRows = CurrentLevelParameters.CountOfRows;
                boxesOnRow = CurrentLevelParameters.BoxesOnRow;
                break;
            case 2:
                int indexSize = Random.Range(0, QuickGamePanel.QuickGameSizes.Length);
                Vector2Int randomSize = QuickGamePanel.QuickGameSizes[indexSize];

                int indexCycle = Random.Range(0, QuickGamePanel.QuickGameCycleTypes.Length);
                int randomCycleType = QuickGamePanel.QuickGameCycleTypes[indexCycle];
                if (randomCycleType == 0) {
                    countOfCycles = 0;
                }
                else if (randomCycleType == 1) {
                    countOfCycles = Random.Range(1, 3);
                }
                else if (randomCycleType == 2) {
                    countOfCycles = Random.Range(2, 4);
                }

                countOfRows = randomSize.x;
                boxesOnRow = randomSize.y;

                if ((countOfRows == 2 && boxesOnRow == 3 || countOfRows == 3 && boxesOnRow == 4) && Random.Range(0, 2) == 0) {
                    int temp = countOfRows;
                    countOfRows = boxesOnRow;
                    boxesOnRow = temp;
                }

                int maxCycleCount = countOfRows * boxesOnRow - countOfRows - boxesOnRow + 1;
                if (countOfCycles > maxCycleCount) {
                    countOfCycles = maxCycleCount;
                }
                break;
            case 3:
                int randomNumber = Random.Range(0, 10);
                if (randomNumber == 9) {
                    countOfCycles = 2;
                }
                else if (randomNumber >= 6) {
                    countOfCycles = 1;
                }
                else {
                    countOfCycles = 0;
                }
                countOfRows = Random.Range(4, 7);
                if (Random.Range(0, 4) != 0) {
                    boxesOnRow = Random.Range(3, 5);
                }
                else {
                    boxesOnRow = Random.Range(3, 8);
                }
                break;
            case 4:
                LevelParametersGenerator.GenerateNextLevelParameters(levelNumber);
                countOfCycles = LevelParametersGenerator.CountOfCycle;
                countOfRows = LevelParametersGenerator.CurrentLevelParameters.CountOfRows;
                boxesOnRow = LevelParametersGenerator.CurrentLevelParameters.BoxesOnRow;
                break;
        }
        if (countOfRows % 2 == 1 && boxesOnRow % 2 == 1) {
            if (Random.Range(0, 2) == 0) {
                if (Random.Range(0, 2) == 0) {
                    countOfRows++;
                }
                else {
                    countOfRows--;
                }
            }
            else {
                if (boxesOnRow == 7) {
                    boxesOnRow--;
                }
                else {
                    if (Random.Range(0, 2) == 0) {
                        boxesOnRow++;
                    }
                    else {
                        boxesOnRow--;
                    }
                }
            }
        }
        
        if (boxesOnRow < 5) {
            conveyorsPositions = new int[countOfRows * 2 - 1, boxesOnRow * 2 - 1];
        }
        else {
            conveyorsPositions = new int[countOfRows * 2 - 1, boxesOnRow];
        }
        for (int i = 0; i < conveyorsPositions.GetLength(0); i++) {
            for (int j = 0; j < conveyorsPositions.GetLength(1); j++) {
                if (i % 2 == 0) {
                    conveyorsPositions[i, j] = 1;
                }
                else {
                    conveyorsPositions[i, j] = 0;
                }
            }
        }
        
        if (boxesOnRow < 5) {
            boxesId = new int[countOfRows * 2 - 1, boxesOnRow * 2 - 1];
        }
        else {
            boxesId = new int[countOfRows * 2 - 1, boxesOnRow];
        }
        int boxIndex = 0;
        for (int i = 0; i < boxesId.GetLength(0); i++) {
            for (int j = 0; j < boxesId.GetLength(1); j++) {
                if (i % 2 == 0) {
                    if (boxesOnRow < 5) {
                        if (j % 2 == 0) {
                            boxesId[i, j] = boxIndex;
                            boxIndex++;
                        }
                        else {
                            boxesId[i, j] = -1;
                        }
                    }
                    else {
                        boxesId[i, j] = boxIndex;
                        boxIndex++;
                    }
                }
                else {
                    boxesId[i, j] = -1;
                }
            }
        }

        List<int[]> cyclesList = new List<int[]>();
        List<Vector2Int> cyclesSizeList = new List<Vector2Int>();
        int rows = countOfRows;
        int boxes = boxesOnRow;

        while (rows > 1) {
            int cycleN = (rows * boxes) - ((rows - 2) * (boxes - 2));
            int[] cycleR = new int[0];

            for (int startI = 0; startI < countOfRows - rows + 1; startI++) {
                for (int startJ = 0; startJ < boxesOnRow - boxes + 1; startJ++) {

                    cycleR = new int[cycleN];
                    cyclesSizeList.Add(new Vector2Int(rows, boxes));
                    int indexR = 0;

                    for (int j = startJ; j < startJ + boxes; j++) {
                        cycleR[indexR] = startI * boxesOnRow + j;
                        indexR++;
                    }
                    for (int i = startI + 1; i < startI + rows; i++) {
                        cycleR[indexR] = i * boxesOnRow + (startJ + boxes - 1);
                        indexR++;
                    }
                    for (int j = startJ + boxes - 2; j >= startJ; j--) {
                        cycleR[indexR] = (startI + rows - 1) * boxesOnRow + j;
                        indexR++;
                    }
                    for (int i = startI + rows - 2; i >= startI + 1; i--) {
                        cycleR[indexR] = i * boxesOnRow + startJ;
                        indexR++;
                    }
                    cyclesList.Add(cycleR);
                }
            }

            boxes--;
            if (boxes < 2) {
                rows--;
                boxes = boxesOnRow;
            }
        }

        levelCycles = new Cycle[countOfCycles];
        cycle = new int[countOfCycles][,];
        startCycleCoords = new Vector2Int[countOfCycles];

        for (int i = 0; i < countOfCycles; i++) {

            int cycleIndex = -1;
            if (gameMode == 1 || gameMode == 0) {
                for (int gi = 0; gi < cyclesSizeList.Count; gi++) {
                    if (cyclesSizeList[gi] == CurrentLevelParameters.CyclesSize[i] && cyclesList[gi][0] == CurrentLevelParameters.StartCycleBoxId[i]) {
                        cycleIndex = gi;
                    }
                }
            }
            else if (gameMode == 2 && i == 1 && countOfCycles == 2) {
                int maxSizeOfSecondCycle = 2;
                if (countOfRows * boxesOnRow > 24) {
                    maxSizeOfSecondCycle = 3;
                }
                for (int ci = 0; ci < cyclesSizeList.Count; ci++) {
                    if (cyclesSizeList[ci].x > maxSizeOfSecondCycle || cyclesSizeList[ci].y > maxSizeOfSecondCycle) {
                        cyclesList.RemoveAt(ci);
                        cyclesSizeList.RemoveAt(ci);
                        ci--;
                    }
                }
                cycleIndex = Random.Range(0, cyclesSizeList.Count);
            }
            else {
                cycleIndex = Random.Range(0, cyclesSizeList.Count);
            }

            int[] currentCycle = new int[cyclesList[cycleIndex].Length];
            cyclesList[cycleIndex].CopyTo(currentCycle, 0);

            Vector2Int countBoxesInCycle = cyclesSizeList[cycleIndex];

            for (int ci = 0; ci < cyclesList.Count; ci++) {
                if (cyclesList[ci][0] == currentCycle[0]) {
                    cyclesList.RemoveAt(ci);
                    cyclesSizeList.RemoveAt(ci);
                    ci--;
                }
            }
            if (boxesOnRow < 5) {
                cycle[i] = new int[countBoxesInCycle.x * 2 - 1, countBoxesInCycle.y * 2 - 1];
            }
            else {
                cycle[i] = new int[countBoxesInCycle.x * 2 - 1, countBoxesInCycle.y];
            }
            for (int ci = 0; ci < cycle[i].GetLength(0); ci++) {
                for (int cj = 0; cj < cycle[i].GetLength(1); cj++) {
                    if (ci == 0 || ci == cycle[i].GetLength(0) - 1) {
                        if (cj == 0 || cj == cycle[i].GetLength(1) - 1) {
                            cycle[i][ci, cj] = 3;
                        }
                        else {
                            cycle[i][ci, cj] = 1;
                        }
                    }
                    else {
                        if (cj == 0 || cj == cycle[i].GetLength(1) - 1) {
                            cycle[i][ci, cj] = 4;
                        }
                        else {
                            cycle[i][ci, cj] = 0;
                        }
                    }
                }
            }

            int spawnPositionX = 0;
            int startValue = currentCycle[0];
            while (startValue >= boxesOnRow) {
                spawnPositionX++;
                startValue -= boxesOnRow;
            }
            int spawnPositionY = currentCycle[0] % boxesOnRow;
            if (boxesOnRow < 5) {
                startCycleCoords[i] = new Vector2Int(spawnPositionX * 2, spawnPositionY * 2);
            }
            else {
                startCycleCoords[i] = new Vector2Int(spawnPositionX * 2, spawnPositionY);
            }
            int cycleN = (countBoxesInCycle.x * countBoxesInCycle.y) - ((countBoxesInCycle.x - 2) * (countBoxesInCycle.y - 2));
            levelCycles[i] = new Cycle();
            levelCycles[i].BoxesIdForCycle = new int[cycleN];
        }
        
        for (int r = 0; r < cycle.Length; r++) {
            for (int i = 0, x = startCycleCoords[r].x; i < cycle[r].GetLength(0); i++, x++) {
                for (int j = 0, y = startCycleCoords[r].y; j < cycle[r].GetLength(1); j++, y++) {
                    if (cycle[r][i, j] == 4 && conveyorsPositions[x, y] == 1) {
                        conveyorsPositions[x, y] = 3;
                    }
                    else if (cycle[r][i, j] == 0 && conveyorsPositions[x, y] != 0) {
                        continue;
                    }
                    else if (cycle[r][i, j] == 4 && conveyorsPositions[x, y] != 0) {
                        continue;
                    }
                    else if (cycle[r][i, j] == 1 && conveyorsPositions[x, y] == 3) {
                        continue;
                    }
                    else {
                        conveyorsPositions[x, y] = cycle[r][i, j];
                    }
                }
            }
        }
        
        for (int i = 0; i < conveyorsPositions.GetLength(0); i++) {
            for (int j = 0; j < conveyorsPositions.GetLength(1); j++) {
                if (conveyorsPositions[i, j] == 1) {
                    int count = 0;
                    while (conveyorsPositions[i, j] == 1) {
                        count++;
                        if (j + 1 >= conveyorsPositions.GetLength(1)) {
                            break;
                        }
                        else if (conveyorsPositions[i, j + 1] != 1) {
                            break;
                        }
                        else {
                            j++;
                        }
                    }
                    if (count == 1) {
                        conveyorsPositions[i, j] = 2;
                    }
                }
            }
        }

        SpawnConveyors(levelNumber);
        InitBoxesPosition();
    }

    private void SpawnConveyors(int levelNumber) {
        int n = 0;
        for (int i = 0; i < conveyorsPositions.GetLength(0); i++) {
            for (int j = 0; j < conveyorsPositions.GetLength(1); j++) {
                if (conveyorsPositions[i, j] == 1) {
                    if (j + 1 >= conveyorsPositions.GetLength(1) || j + 1 < conveyorsPositions.GetLength(1) && conveyorsPositions[i, j + 1] != 1) {
                        n++;
                    }
                }
                else if (conveyorsPositions[i, j] != 0) {
                    n++;
                }
            }
        }
        conveyorsAnimation = new ConveyorLevelAnimation[n];

        int index = (conveyorsCoordinatesY.Length - countOfRows) / 2;
        float z = 7f;

        int conveyorIndex = 0;

        float offsetByBoxCount = 0;
        if (boxesOnRow % 2 == 0 && boxesOnRow > 4) {
            offsetByBoxCount = 0.46f;
        }

        float startX = (conveyorsPositions.GetLength(1) - Mathf.CeilToInt(conveyorsPositions.GetLength(1) / 2f)) * (-0.92f) + spawnOffsetX + offsetByBoxCount;
        float[] x = new float[conveyorsPositions.GetLength(1)];
        for (int i = 0; i < conveyorsPositions.GetLength(1); i++) {
            x[i] = startX + i * 0.92f;
        }

        GameObject conveyor;
        spawners = new GameObject[countOfRows];
        int spawnersIndex = 0;
        int[,] conveyorsTypesWithIndex = new int[conveyorsPositions.GetLength(0), conveyorsPositions.GetLength(1)];

        for (int i = 0; i < conveyorsPositions.GetLength(0); i++) {
            for (int j = 0; j < conveyorsPositions.GetLength(1); j++) {
                switch (conveyorsPositions[i, j]) {
                    case 0:
                        conveyorsTypesWithIndex[i, j] = -1;
                        continue;
                    case 1:
                        int count = 0;
                        float conveyorStartX = x[j];
                        while (conveyorsPositions[i, j] == 1) {
                            count++;
                            conveyorsTypesWithIndex[i, j] = conveyorIndex;
                            if (j + 1 >= conveyorsPositions.GetLength(1)) {
                                break;
                            }
                            else {
                                j++;
                            }
                        }
                        if (conveyorsPositions[i, j] != 1) {
                            j--;
                        }
                        conveyor = Instantiate(threePlaceConveyor, new Vector3((conveyorStartX + x[j]) / 2, conveyorsCoordinatesY[index], z), Quaternion.identity);
                        conveyorsAnimation[conveyorIndex] = conveyor.GetComponent<ConveyorLevelAnimation>();
                        conveyorsAnimation[conveyorIndex].ConveyorType = 1;
                        conveyor.GetComponent<SpriteRenderer>().size += new Vector2((count - 3) * 0.46f, 0);
                        break;
                    case 2:
                        conveyor = Instantiate(onePlaceConveyor, new Vector3(x[j], conveyorsCoordinatesY[index], z), Quaternion.identity);
                        conveyorsAnimation[conveyorIndex] = conveyor.GetComponent<ConveyorLevelAnimation>();
                        conveyorsAnimation[conveyorIndex].ConveyorType = 2;
                        conveyorsTypesWithIndex[i, j] = conveyorIndex;
                        break;
                    case 3:
                        conveyor = Instantiate(angleConveyor, new Vector3(x[j], conveyorsCoordinatesY[index], z), Quaternion.identity);
                        conveyorsAnimation[conveyorIndex] = conveyor.GetComponent<ConveyorLevelAnimation>();
                        conveyorsAnimation[conveyorIndex].ConveyorType = 3;
                        conveyorsTypesWithIndex[i, j] = conveyorIndex;
                        break;
                    case 4:
                        conveyor = Instantiate(verticalConveyor, new Vector3(x[j], conveyorsCoordinatesY[index] + 0.92f, z), Quaternion.identity);
                        conveyorsAnimation[conveyorIndex] = conveyor.GetComponent<ConveyorLevelAnimation>();
                        conveyorsAnimation[conveyorIndex].ConveyorType = 4;
                        conveyorsTypesWithIndex[i, j] = conveyorIndex;
                        break;
                }
                conveyorIndex++;
            }
            if (i % 2 == 0) {
                if (levelNumber == 0) {
                    float spawnerOffset = boxesOnRow * 2 - 2;
                    if (boxesOnRow > 4) {
                        spawnerOffset = boxesOnRow - 1;
                    }
                    spawners[spawnersIndex] = Instantiate(spawner, new Vector3(0.65f + spawnerOffset * 0.46f + spawnOffsetX, conveyorsCoordinatesY[index] + 0.36f, -10), Quaternion.identity);
                    spawnersIndex++;
                }
                index++;
            }
            z -= 0.5f;
        }

        int boxIndex = 0;
        List<int> conveyorsInCycleList = new List<int>();
        for (int r = 0; r < cycle.Length; r++) {
            int conveyorTypeIndex = -1;
            for (int j = startCycleCoords[r].y; j < startCycleCoords[r].y + cycle[r].GetLength(1); j++) {
                conveyorTypeIndex = conveyorsTypesWithIndex[startCycleCoords[r].x, j];
                conveyorsInCycleList.Add(conveyorTypeIndex);

                if (j == startCycleCoords[r].y) {
                    conveyorsAnimation[conveyorTypeIndex].AddCornerAndVergeIds(1, 1, r);
                }
                else if (j + 1 >= startCycleCoords[r].y + cycle[r].GetLength(1)) {
                    conveyorsAnimation[conveyorTypeIndex].AddCornerAndVergeIds(2, 1, r);
                }
                else if (conveyorsPositions[startCycleCoords[r].x, j] == 3) {
                    conveyorsAnimation[conveyorTypeIndex].AddCornerAndVergeIds(-1, 1, r);
                }

                if (boxesId[startCycleCoords[r].x, j] != -1) {
                    levelCycles[r].BoxesIdForCycle[boxIndex] = boxesId[startCycleCoords[r].x, j];
                    boxIndex++;
                }
            }
            for (int i = startCycleCoords[r].x + 1; i < startCycleCoords[r].x + cycle[r].GetLength(0); i++) {
                conveyorTypeIndex = conveyorsTypesWithIndex[i, startCycleCoords[r].y + cycle[r].GetLength(1) - 1];
                conveyorsInCycleList.Add(conveyorTypeIndex);
                if (boxesId[i, startCycleCoords[r].y + cycle[r].GetLength(1) - 1] != -1) {
                    levelCycles[r].BoxesIdForCycle[boxIndex] = boxesId[i, startCycleCoords[r].y + cycle[r].GetLength(1) - 1];
                    boxIndex++;
                }

                if (i + 1 >= startCycleCoords[r].x + cycle[r].GetLength(0)) {
                    conveyorsAnimation[conveyorTypeIndex].AddCornerAndVergeIds(3, 2, r);
                }
                else if (conveyorsPositions[i, startCycleCoords[r].y + cycle[r].GetLength(1) - 1] == 3) {
                    conveyorsAnimation[conveyorTypeIndex].AddCornerAndVergeIds(-1, 2, r);
                }

                if (levelCycles[r].ChangeDirectoryConveyorId == 0 && i + 1 >= startCycleCoords[r].x + cycle[r].GetLength(0)) {
                    levelCycles[r].ChangeDirectoryConveyorId = conveyorTypeIndex;
                }
            }
            for (int j = startCycleCoords[r].y + cycle[r].GetLength(1) - 2; j >= startCycleCoords[r].y; j--) {
                conveyorTypeIndex = conveyorsTypesWithIndex[startCycleCoords[r].x + cycle[r].GetLength(0) - 1, j];
                conveyorsInCycleList.Add(conveyorTypeIndex);
                if (boxesId[startCycleCoords[r].x + cycle[r].GetLength(0) - 1, j] != -1) {
                    levelCycles[r].BoxesIdForCycle[boxIndex] = boxesId[startCycleCoords[r].x + cycle[r].GetLength(0) - 1, j];
                    boxIndex++;
                }

                if (j - 1 < startCycleCoords[r].y) {
                    conveyorsAnimation[conveyorTypeIndex].AddCornerAndVergeIds(4, 3, r);
                }
                else if (conveyorsPositions[startCycleCoords[r].x + cycle[r].GetLength(0) - 1, j] == 3) {
                    conveyorsAnimation[conveyorTypeIndex].AddCornerAndVergeIds(-1, 3, r);
                }
            }
            for (int i = startCycleCoords[r].x + cycle[r].GetLength(0) - 2; i >= startCycleCoords[r].x + 1; i--) {
                conveyorTypeIndex = conveyorsTypesWithIndex[i, startCycleCoords[r].y];
                conveyorsInCycleList.Add(conveyorTypeIndex);
                if (boxesId[i, startCycleCoords[r].y] != -1) {
                    levelCycles[r].BoxesIdForCycle[boxIndex] = boxesId[i, startCycleCoords[r].y];
                    boxIndex++;
                }

                if (conveyorsPositions[i, startCycleCoords[r].y] == 3) {
                    conveyorsAnimation[conveyorTypeIndex].AddCornerAndVergeIds(-1, 4, r);
                }
            }

            levelCycles[r].ConveyorsInCycle = new int[conveyorsInCycleList.Count];
            levelCycles[r].ConveyorsInCycle = conveyorsInCycleList.ToArray();

            conveyorsInCycleList.Clear();
            boxIndex = 0;
        }
    }

    private void InitBoxesPosition() {
        
        boxesPosition = new Vector3[countOfRows * boxesOnRow];
        int indexY = (conveyorsCoordinatesY.Length - countOfRows) / 2;
        float offset;
        if (boxesOnRow % 2 == 0) {
            offset = 0.92f;
        }
        else {
            offset = 0;
        }

        if (boxesOnRow % 2 == 0 && boxesOnRow > 4) {
            offset -= 0.46f;
        }

        float startPointOffset = -1.84f;
        if (boxesOnRow > 4) {
            startPointOffset = -0.92f;
        }

        float startX = (boxesOnRow - Mathf.CeilToInt(boxesOnRow / 2f)) * startPointOffset + offset;
        float[] x = new float[boxesOnRow];

        float offsetByBoxCount = 1.84f;
        if (boxesOnRow > 4) {
            offsetByBoxCount = 0.92f;
        }

        for (int i = 0; i < boxesOnRow; i++) {
            x[i] = startX + i * offsetByBoxCount;
        }

        for (int i = 0; i < countOfRows; i++) {
            for (int j = 0; j < boxesOnRow; j++) {
                boxesPosition[i * boxesOnRow + j] = new Vector3(x[j], conveyorsCoordinatesY[indexY] + 0.36f, -10);
            }
            indexY++;
        }
    }

    public Level AddLevel() {

        Level level = gameObject.AddComponent<Level>();

        int[] places = new int[countOfRows];
        for (int i = 0; i < countOfRows; i++) {
            places[i] = boxesOnRow;
        }

        level.InitLevel(CurrentLevelParameters.LevelNumber, countOfRows, places, boxesPosition, conveyorsAnimation, levelCycles, spawners);
        return level;
    }
}

public class CurrentLevelParameters {
    public static int LevelNumber { get; set; }
    public static int CountOfRows { get; set; }
    public static int BoxesOnRow { get; set; }
    public static int[] StartCycleBoxId { get; set; }
    public static Vector2Int[] CyclesSize { get; set; }
    public static int TopLevelMoves { get; set; }
    public static float TopLevelTime { get; set; }
}
