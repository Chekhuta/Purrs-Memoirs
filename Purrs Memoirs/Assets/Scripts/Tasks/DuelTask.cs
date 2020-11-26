[System.Serializable]
public class DuelTask : Task {

    public static Task CreateTask() {
        return new DuelTask(5, 80);
    }

    private DuelTask(int taskSteps, int taskReward) : base(new TaskType(TaskTypeName.DuelTask), taskSteps, taskReward) { }

    public override string GetDescription() {
        if (DataStorage.LanguageId == 0) {
            return "Win  " + steps + "  duels";
        }
        else if (DataStorage.LanguageId == 1) {
            return "Одержите  победу\nв  " + steps + "  дуэлях";
        }
        else if (DataStorage.LanguageId == 2) {
            return "Здобудьте перемогу\nв  " + steps + "  дуелях";
        }
        else {
            return "Win  " + steps + "  duels";
        }
    }
}