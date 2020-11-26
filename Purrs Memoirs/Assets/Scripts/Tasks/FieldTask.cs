using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class FieldTask : Task {

    public static Task CreateTask() {
        List<Vector2Int> levelSizes = new List<Vector2Int> {
            new Vector2Int(2, 3), new Vector2Int(3, 4), new Vector2Int(4, 4),
            new Vector2Int(5, 4), new Vector2Int(6, 4), new Vector2Int(6, 7)
        };
        List<int> rewards = new List<int> { 60, 80, 100, 100, 90, 100 };
        List<int> stepsVariants = new List<int> { 10, 10, 10, 8, 6, 5 };

        for (int i = 0; i < TasksData.Tasks.Length; i++) {
            if (TasksData.Tasks[i] != null) {
                if (TasksData.Tasks[i].GetTaskTypeName() == TaskTypeName.FieldTask) {
                    int j = levelSizes.IndexOf(((FieldTask)TasksData.Tasks[i]).GetFieldSize());
                    rewards.RemoveAt(j);
                    levelSizes.RemoveAt(j);
                    stepsVariants.RemoveAt(j);
                }
            }
        }
        int index = Random.Range(0, levelSizes.Count);
        Vector2Int fieldSize = levelSizes[index];
        int steps = stepsVariants[index];
        int reward = rewards[index];
        return new FieldTask(fieldSize.x, fieldSize.y, steps, reward);
    }

    private FieldTask(int sizeX, int sizeY, int taskSteps, int taskReward) : base(new FieldTaskType(TaskTypeName.FieldTask, sizeX, sizeY), taskSteps, taskReward) {
    }

    public override string GetDescription() {
        
        int x = ((FieldTaskType)taskType).GetFieldSize().x;
        int y = ((FieldTaskType)taskType).GetFieldSize().y;
        if (x > y) {
            int buf = x;
            x = y;
            y = buf;
        }
        if (DataStorage.LanguageId == 0) {
            return "Complete  " + steps + "  levels\non  " + x + " x " + y + "  field";
        }
        else if (DataStorage.LanguageId == 1) {
            return "Пройдите  " + steps + "  уровней\nна  " + x + " x " + y + "  поле";
        }
        else if (DataStorage.LanguageId == 2) {
            return "Пройдiть  " + steps + "  рiвнiв\nна  " + x + " x " + y + "  полi";
        }
        else {
            return "Complete  " + steps + "  levels\non  " + x + " x " + y + "  field";
        }
    }

    private Vector2Int GetFieldSize() {
        return ((FieldTaskType)taskType).GetFieldSize();
    }
}