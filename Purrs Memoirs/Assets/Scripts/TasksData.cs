using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class TasksData : MonoBehaviour {

    public static int Coins { get; set; } = 0;
    public static Task[] Tasks { get; set; }
    public static bool IsTutorialCompleted { get; set; } = false;
    private static bool IsInitialized;

    public static void SaveTasksData() {
        TasksDataSer data = new TasksDataSer();
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/tdcbc.bin";
        FileStream fileStream = new FileStream(path, FileMode.Create);
        formatter.Serialize(fileStream, data);
        fileStream.Close();
    }

    public static void LoadTasksData() {
        if (IsInitialized) {
            return;
        }
        string path = Application.persistentDataPath + "/tdcbc.bin";
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);
            TasksDataSer data = formatter.Deserialize(fileStream) as TasksDataSer;
            fileStream.Close();
            Tasks = data.tasksSer;
            Coins = data.coinsSer;
            IsTutorialCompleted = data.isTutorialCompletedSer;
        }
        else {
            InitializeTasks();
            SaveTasksData();
        }
        IsInitialized = true;
    }

    public static void PayToContinueGame(int price) {
        Coins -= price;
        SaveTasksData();
    }

    public static bool IsCompletedTasks() {
        for (int i = 0; i < Tasks.Length; i++) {
            if (Tasks[i].IsTaskCompleted()) {
                return true;
            }
        }
        return false;
    }

    public static void TutorialCompleted() {
        IsTutorialCompleted = true;
        UpdateTaskProgress(new TaskType(TaskTypeName.TutorialTask), 1);
        SaveTasksData();
    }

    public static void UpdateTaskProgress(TaskType taskType, int progress) {
        for (int i = 0; i < Tasks.Length; i++) {
            if (Tasks[i].CompareTasksType(taskType) && Tasks[i].CanAddProgress(progress)) {
                Tasks[i].AddProgress(progress);
            }
        }
    }
    
    public static void GiveNewTask(int taskIndex) {
        CreateTask(taskIndex);
        SaveTasksData();
    }

    private static void InitializeTasks() {
        Tasks = new Task[3];
        for (int i = 0; i < Tasks.Length; i++) {
            if (i == 0 && !IsTutorialCompleted) {
                Tasks[i] = TutorialTask.CreateTask();
            }
            else {
                CreateTask(i);
            }
        }
    }
    
    private static void CreateTask(int taskIndex) {
        List<TaskTypeName> availableTasksTypes = new List<TaskTypeName>() { TaskTypeName.FindCatTask, TaskTypeName.FindPairsTask, TaskTypeName.DuelTask, TaskTypeName.TimeAttackTask, TaskTypeName.FieldTask };
        for (int i = 0; i < Tasks.Length; i++) {
            if (Tasks[i] != null) {
                if (Tasks[i].GetTaskTypeName() == TaskTypeName.FindPairsTask || Tasks[i].GetTaskTypeName() == TaskTypeName.DuelTask || Tasks[i].GetTaskTypeName() == TaskTypeName.TimeAttackTask) {
                    availableTasksTypes.Remove(Tasks[i].GetTaskTypeName());
                }
            }
        }

        TaskTypeName taskType = availableTasksTypes[Random.Range(0, availableTasksTypes.Count)];
        switch (taskType) {
            case TaskTypeName.FindCatTask:
                Tasks[taskIndex] = FindCatTask.CreateTask();
                break;
            case TaskTypeName.FindPairsTask:
                Tasks[taskIndex] = FindPairsTask.CreateTask();
                break;
            case TaskTypeName.DuelTask:
                Tasks[taskIndex] = DuelTask.CreateTask();
                break;
            case TaskTypeName.TimeAttackTask:
                Tasks[taskIndex] = TimeAttackTask.CreateTask();
                break;
            case TaskTypeName.FieldTask:
                Tasks[taskIndex] = FieldTask.CreateTask();
                break;
        }
    }
}

[System.Serializable]
class TasksDataSer {
    public Task[] tasksSer;
    public int coinsSer;
    public bool isTutorialCompletedSer;

    public TasksDataSer() {
        tasksSer = TasksData.Tasks;
        coinsSer = TasksData.Coins;
        isTutorialCompletedSer = TasksData.IsTutorialCompleted;
    }
}