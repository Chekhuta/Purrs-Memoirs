using UnityEngine;

[System.Serializable]
public class TimeAttackTask : Task {

    public static Task CreateTask() {
        int index = Random.Range(0, 2);
        int steps = new int[] { 20, 30 }[index];
        int reward = new int[] { 60, 80 }[index];
        return new TimeAttackTask(steps, reward);
    }

    private TimeAttackTask(int taskSteps, int taskReward) : base(new TaskType(TaskTypeName.TimeAttackTask), taskSteps, taskReward) { }

    public override string GetDescription() {
        if (DataStorage.LanguageId == 0) {
            return "Find  " + steps + "  pairs  in\ntime  attack  mode";
        }
        else if (DataStorage.LanguageId == 1) {
            return "Найдите  " + steps + "  пар  в\nигре  на  время";
        }
        else if (DataStorage.LanguageId == 2) {
            return "Знайдiть  " + steps + "  пар  в\nгрi  на  час";
        }
        else {
            return "Find  " + steps + "  pairs  in\ntime  attack  mode";
        }
    }

    public override string GetProgress() {
        if (!IsTaskCompleted()) {
            return "0 / 1";
        }
        else {
            return LanguageTitles.GetInstance().rewardTitle;
        }
    }

    public override bool CanAddProgress(int progress) {
        if (!IsTaskCompleted() && progress >= steps) {
            return true;
        }
        else {
            return false;
        }
    }
}