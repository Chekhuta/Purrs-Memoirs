using System.Collections.Generic;
using UnityEngine;

public class LevelParametersGenerator : MonoBehaviour {

    private static List<LevelParameters> TimeAttackLevels { get; set; } = new List<LevelParameters>() {
        new LevelParameters(2, 2, 5),
        new LevelParameters(3, 2, 8),
        new LevelParameters(5, 2, 12),
        new LevelParameters(4, 3, 16),
        new LevelParameters(4, 4, 24),
        new LevelParameters(6, 3, 30),
        new LevelParameters(5, 4, 38),
        new LevelParameters(6, 4, 52)
    };

    public class LevelParameters {
        public int CountOfRows { get; set; }
        public int BoxesOnRow { get; set; }
        public int BonusLevelTime { get; set; }

        public LevelParameters(int rows, int boxes, int bonusTime) {
            CountOfRows = rows;
            BoxesOnRow = boxes;
            BonusLevelTime = bonusTime;
        }
    }
    public static LevelParameters CurrentLevelParameters = TimeAttackLevels[0];
    public static int CountOfCycle { get; set; } = 0;

    public static void GenerateNextLevelParameters(int levelNumber) {
        if (levelNumber < 7) {
            CurrentLevelParameters = TimeAttackLevels[levelNumber];
        }
        else {
            CurrentLevelParameters = TimeAttackLevels[7];
        }
        if (levelNumber > 4 && levelNumber % 2 == 0 && Random.Range(0, 2) == 0) {
            CountOfCycle = 1;
        }
        else {
            CountOfCycle = 0;
        }
    }

}
