using UnityEngine;

[System.Serializable]
public class FindPairsTask : Task {

    public static Task CreateTask() {
        int steps = new int[] { 50, 100, 200 }[Random.Range(0, 3)];
        int reward = steps;
        return new FindPairsTask(steps, reward);
    }

    private FindPairsTask(int taskSteps, int taskReward) : base(new TaskType(TaskTypeName.FindPairsTask), taskSteps, taskReward) { }

    public override string GetDescription() {
        if (DataStorage.LanguageId == 0) {
            return "Find  " + steps + "  pairs";
        }
        else if (DataStorage.LanguageId == 1) {
            return "Найдите  " + steps + "  пар";
        }
        else if (DataStorage.LanguageId == 2) {
            return "Знайдiть  " + steps + "  пар";
        }
        else {
            return "Find  " + steps + "  pairs";
        }
    }
}