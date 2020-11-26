using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataStorage : MonoBehaviour {

    public static int[] StarsForLevel { get; set; } =  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                         0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                         0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public static int TimeAttackScore { get; set; } = 0;
    public static int NPA { get; set; } = -2;
    public static bool IsEEA { get; set; }
    public static int LanguageId { get; set; } = 0;
    public static bool Sound { get; set; } = true;
    public static bool Music { get; set; } = true;
    public static int RateGameShowCount { get; set; } = 0;

    public static void AddRateGameShow(int value) {
        RateGameShowCount += value;
        SaveDataStorage();
    }

    public static void SetSound(bool value) {
        Sound = value;
        SaveDataStorage();
    }

    public static string GetLanguageString() {
        switch(LanguageId) {
            case 0:
                return "eng";
            case 1:
                return "rus";
            case 2:
                return "ukr";
            default:
                return "eng";
        }
    }

    public static void SetMusic(bool value) {
        Music = value;
        SaveDataStorage();
    }

    public static void NextLanguage() {
        LanguageId++;
        if (LanguageId >= 3) {
            LanguageId = 0;
        }
        SaveDataStorage();
    }

    public static void SetStarsForLevel (int level, int stars) {
        StarsForLevel[level - 1] = stars;
        SaveDataStorage();
    }

    public static void SetTimeAttackBestScore(int score) {
        TimeAttackScore = score;
        SaveDataStorage();
    }

    public static void SetNPA(int npaValue) {
        NPA = npaValue;
        SaveDataStorage();
    }

    public static int GetCountOfStars(int level) {
        return StarsForLevel[level - 1];
    }

    public static int GetSumOfStars() {
        int sum = 0;
        for (int i = 0; i < StarsForLevel.Length; i++) {
            sum += StarsForLevel[i];
        }
        return sum;
    }

    public static void SaveDataStorage() {
        DataStorageSer data = new DataStorageSer();
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/dscbc.bin";
        FileStream fileStream = new FileStream(path, FileMode.Create);
        formatter.Serialize(fileStream, data);
        fileStream.Close();
    }

    public static void LoadDataStorage() {
        string path = Application.persistentDataPath + "/dscbc.bin";
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);
            DataStorageSer data = formatter.Deserialize(fileStream) as DataStorageSer;
            fileStream.Close();
            TimeAttackScore = data.TimeAttackScoreSer;
            StarsForLevel = data.StarsForLevelSer;
            NPA = data.NPASer;
            LanguageId = data.LanguageIdSer;
            Sound = data.SoundSer;
            Music = data.MusicSer;
            RateGameShowCount = data.RateGameShowCountSer;
        }
        else {
            SystemLanguage lang = SystemLanguage.English;
            lang = Application.systemLanguage;
            if (lang == SystemLanguage.Russian) {
                LanguageId = 1;
            }
            else if (lang == SystemLanguage.Ukrainian) {
                LanguageId = 2;
            }
            else {
                LanguageId = 0;
            }
        }
    }
}

[System.Serializable]
class DataStorageSer {

    public int TimeAttackScoreSer;
    public int[] StarsForLevelSer;
    public int NPASer;
    public int LanguageIdSer;
    public bool SoundSer;
    public bool MusicSer;
    public int RateGameShowCountSer;
    public DataStorageSer() {
        TimeAttackScoreSer = DataStorage.TimeAttackScore;
        StarsForLevelSer = DataStorage.StarsForLevel;
        NPASer = DataStorage.NPA;
        LanguageIdSer = DataStorage.LanguageId;
        SoundSer = DataStorage.Sound;
        MusicSer = DataStorage.Music;
        RateGameShowCountSer = DataStorage.RateGameShowCount;
    }
}
