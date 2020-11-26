using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class FindCatTask : Task {

    public static Task CreateTask() {
        int steps = new int[] { 3, 4, 5 }[Random.Range(0, 3)];
        List<int> catsId = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
        for (int i = 0; i < TasksData.Tasks.Length; i++) {
            if (TasksData.Tasks[i] != null) {
                if (TasksData.Tasks[i].GetTaskTypeName() == TaskTypeName.FindCatTask) {
                    catsId.Remove(((FindCatTask)TasksData.Tasks[i]).GetCatId());
                }
            }
        }

        int catId = catsId[Random.Range(0, catsId.Count)];
        int reward = steps * 20;
        return new FindCatTask(catId, steps, reward);
    }

    private FindCatTask(int id, int taskSteps, int taskReward) : base(new FindCatTaskType(TaskTypeName.FindCatTask, id), taskSteps, taskReward) {}

    public override string GetDescription() {
        string pairsVar;
        if (DataStorage.LanguageId == 0) {
            return "Find  " + steps + "  pairs\nof  this  cat";
        }
        else if (DataStorage.LanguageId == 1) {
            if (steps == 5) {
                pairsVar = "пар";
            }
            else {
                pairsVar = "пары";
            }
            return "Найдите  " + steps + "  " + pairsVar + "\nэтого  кота";
        }
        else if (DataStorage.LanguageId == 2) {
            if (steps == 5) {
                pairsVar = "пар";
            }
            else {
                pairsVar = "пари";
            }
            return "Знайдiть  " + steps + "  " + pairsVar + "\nцього  кота";
        }
        else {
            return "Find  " + steps + "  pairs\nof  this  cat";
        }
    }

    public int GetCatId() {
        return ((FindCatTaskType)taskType).GetCatId();
    }
}