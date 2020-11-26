[System.Serializable]
public class TutorialTask : Task {

    public static Task CreateTask() {
        return new TutorialTask(1, 100);
    }

    private TutorialTask(int taskSteps, int taskReward) : base(new TaskType(TaskTypeName.TutorialTask), taskSteps, taskReward) { }

    public override string GetDescription() {
        if (DataStorage.LanguageId == 0) {
            return "Complete  tutorial";
        }
        else if (DataStorage.LanguageId == 1) {
            return "Пройдите\nинструктаж";
        }
        else if (DataStorage.LanguageId == 2) {
            return "Пройдiть\nінструктаж";
        }
        else {
            return "Complete  tutorial";
        }
    }
}