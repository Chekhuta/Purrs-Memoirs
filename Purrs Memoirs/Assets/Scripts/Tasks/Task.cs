using UnityEngine;

[System.Serializable]
public abstract class Task {

    private int progress;
    private int reward;
    protected TaskType taskType;
    protected int steps;

    public Task(TaskType type, int taskSteps, int taskReward) {
        taskType = type;
        steps = taskSteps;
        reward = taskReward;
        progress = 0;
    }

    public virtual string GetProgress() {
        if (!IsTaskCompleted()) {
            return progress + " / " + steps;
        }
        else {
            return LanguageTitles.GetInstance().rewardTitle;
        }
    }

    public TaskTypeName GetTaskTypeName() {
        return taskType.GetTaskTypeName();
    }

    public int GetReward() {
        return reward;
    }

    public int GetSteps() {
        return steps;
    }

    public bool IsTaskCompleted() {
        return progress >= steps;
    }

    public void AddProgress(int countOfSteps) {
        progress += countOfSteps;
    }

    public bool CompareTasksType(TaskType otherTaskType) {
        return taskType.CompareTasksType(otherTaskType);
    }

    public virtual bool CanAddProgress(int progress) {
        if (!IsTaskCompleted()) {
            return true;
        }
        return false;
    }

    public abstract string GetDescription();
}

public enum TaskTypeName {
    TutorialTask,
    FindCatTask,
    FindPairsTask,
    DuelTask,
    TimeAttackTask,
    FieldTask
}

[System.Serializable]
public class TaskType {

    protected TaskTypeName typeName;

    public TaskType(TaskTypeName type) {
        typeName = type;
    }

    public virtual bool CompareTasksType(TaskType otherTaskType) {
        return CompareTypes(otherTaskType);
    }

    public TaskTypeName GetTaskTypeName() {
        return typeName;
    }

    protected bool CompareTypes(TaskType otherTaskType) {
        if (typeName == otherTaskType.typeName) {
            return true;
        }
        return false;
    }
}

[System.Serializable]
public class FindCatTaskType : TaskType {

    private int catId;

    public FindCatTaskType(TaskTypeName type, int cat) : base(type) {
        catId = cat;
    }

    public override bool CompareTasksType(TaskType otherTaskType) {
        return CompareTypes(otherTaskType) && CompareCatsId((FindCatTaskType)otherTaskType);
    }

    public int GetCatId() {
        return catId;
    }

    private bool CompareCatsId(FindCatTaskType otherTaskType) {
        if (catId == otherTaskType.catId) {
            return true;
        }
        return false;
    }
}

[System.Serializable]
public class FieldTaskType : TaskType {

    private int fieldSizeX;
    private int fieldSizeY;

    public FieldTaskType(TaskTypeName type, int sizeX, int sizeY) : base(type) {
        fieldSizeX = sizeX;
        fieldSizeY = sizeY;
    }

    public override bool CompareTasksType(TaskType otherTaskType) {
        return CompareTypes(otherTaskType) && CompareFieldSize((FieldTaskType)otherTaskType);
    }

    public Vector2Int GetFieldSize() {
        return new Vector2Int(fieldSizeX, fieldSizeY);
    }

    private bool CompareFieldSize(FieldTaskType otherTaskType) {
        if (fieldSizeX * fieldSizeY == otherTaskType.fieldSizeX * otherTaskType.fieldSizeY) {
            return true;
        }
        return false;
    }
}