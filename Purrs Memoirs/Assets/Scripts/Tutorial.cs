using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    public GameObject messagePrefab;
    public Transform messageParent;
    public Image background;
    private static bool IsTutorialInitialized;
    private int tutorialStep = 0;
    private TutorialMessage messageWindow;
    
    private void Awake() {
        if (IsTutorialInitialized) {
            Destroy(gameObject);
        }
    }

    public static void SetLevelParameters(int level) {
        if (level == 0) {
            CurrentLevelParameters.CountOfRows = 2;
            CurrentLevelParameters.BoxesOnRow = 3;
            CurrentLevelParameters.StartCycleBoxId = new int[0];
            CurrentLevelParameters.CyclesSize = new Vector2Int[0];
        }
        else if (level == 1) {
            CurrentLevelParameters.CountOfRows = 2;
            CurrentLevelParameters.BoxesOnRow = 4;
            CurrentLevelParameters.StartCycleBoxId = new int[1] { 1 };
            CurrentLevelParameters.CyclesSize = new Vector2Int[1] { new Vector2Int(2, 2) };
        }
    }

    public void SetMessageParent(Transform mParent) {
        messageParent = mParent;
    }

    public void SetMessageBackground(GameObject mBackground) {
        background = mBackground.GetComponent<Image>();
    }

    public void StartTutorial() {
        background.gameObject.SetActive(true);
        background.color += new Color(0, 0, 0, 1);
        messageWindow = Instantiate(messagePrefab, messageParent).GetComponent<TutorialMessage>();
        messageWindow.GetComponent<RectTransform>().localPosition = Vector3.zero;
        messageWindow.SetTutorial(this);
        ShowStartMessage();
    }

    public void ContinueTutorial() {
        background.gameObject.SetActive(true);
        messageWindow = Instantiate(messagePrefab, messageParent).GetComponent<TutorialMessage>();
        messageWindow.GetComponent<RectTransform>().localPosition = Vector3.zero;
        messageWindow.SetTutorial(this);
        messageWindow.OneButtonPattern();
        messageWindow.SetMessage(GetMessageText(tutorialStep));
        messageWindow.SetConfirmButtonText(GetConfirmText(tutorialStep));
    }

    private void ShowStartMessage() {
        messageWindow.SetMessage(GetMessageText(tutorialStep));
        messageWindow.SetConfirmButtonText(GetConfirmText(tutorialStep));
        messageWindow.SetRejectButtonText(GetRejectText());
    }

    public void ConfirmButtonPressed() {
        switch(tutorialStep) {
            case -1:
                background.color -= new Color(0, 0, 0, 1);
                background.gameObject.SetActive(false);
                TasksData.TutorialCompleted();
                Menu menu = FindObjectOfType<Menu>();
                menu.SetActiveButtons(true);
                menu.SetEnabledTaskSign(true);
                Destroy(messageWindow.gameObject);
                break;
            case 0:
                IsTutorialInitialized = true;
                DontDestroyOnLoad(gameObject);
                tutorialStep++;
                Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventTutorialBegin);
                messageWindow.OneButtonPattern();
                messageWindow.SetMessage(GetMessageText(tutorialStep));
                messageWindow.SetConfirmButtonText(GetConfirmText(tutorialStep));
                break;
            case 1:
                tutorialStep++;
                messageWindow.SetMessage(GetMessageText(tutorialStep));
                messageWindow.SetConfirmButtonText(GetConfirmText(tutorialStep));
                messageWindow.ShowPurrImage();
                break;
            case 2:
                tutorialStep++;
                messageWindow.SetMessage(GetMessageText(tutorialStep));
                messageWindow.SetConfirmButtonText(GetConfirmText(tutorialStep));
                messageWindow.ShowPurrImage();
                break;
            case 3:
                tutorialStep++;
                messageWindow.SetMessage(GetMessageText(tutorialStep));
                messageWindow.SetConfirmButtonText(GetConfirmText(tutorialStep));
                messageWindow.ShowPurrImage();
                break;
            case 4:
                tutorialStep++;
                Game.GameMode = 0;
                FindObjectOfType<Transition>().EndScene("Game");
                break;
            case 5:
                tutorialStep++;
                Game.GetInstance().NextSubLevel();
                messageWindow.gameObject.SetActive(false);
                background.gameObject.SetActive(false);
                break;
            case 6:
                messageWindow.gameObject.SetActive(true);
                background.gameObject.SetActive(true);
                messageWindow.SetMessage(GetMessageText(tutorialStep));
                messageWindow.SetConfirmButtonText(GetConfirmText(tutorialStep));
                messageWindow.ShowPurrImage();
                tutorialStep++;
                break;
            case 7:
                TasksData.TutorialCompleted();
                Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventTutorialComplete);
                FindObjectOfType<Transition>().EndScene("Menu");
                Destroy(gameObject);
                break;
        }
    }

    public void RejectButtonPressed() {
        switch (tutorialStep) {
            case 0:
                tutorialStep--;
                messageWindow.SetMessage(MessageReject());
                messageWindow.SetConfirmButtonText(GetCloseText());
                messageWindow.SetRejectButtonText(GetBackText());
                break;
            case -1:
                tutorialStep++;
                ShowStartMessage();
                break;
        }
    }

    public void GameSceneTutorial() {
        StartTutorial();
    }

    public string GetConfirmText(int step) {
        switch (step) {
            case 0:
                return ConfirmButton0();
            case 6:
                return ConfirmButton6();
            default:
                return ConfirmButton1();
        }
    }

    public string ConfirmButton0() {
        if (DataStorage.LanguageId == 0) {
            return "No,  what  happened?";
        }
        else if (DataStorage.LanguageId == 1) {
            return "Не  знаю,  что  случилось?";
        }
        else if (DataStorage.LanguageId == 2) {
            return "Не  знаю,  поясніть";
        }
        return "";
    }

    public string ConfirmButton1() {
        if (DataStorage.LanguageId == 0) {
            return "Next";
        }
        else if (DataStorage.LanguageId == 1) {
            return "Дальше";
        }
        else if (DataStorage.LanguageId == 2) {
            return "Далі";
        }
        return "";
    }

    public string ConfirmButton6() {
        if (DataStorage.LanguageId == 0) {
            return "Close";
        }
        else if (DataStorage.LanguageId == 1) {
            return "Закончить";
        }
        else if (DataStorage.LanguageId == 2) {
            return "Закінчити";
        }
        return "";
    }

    public string GetRejectText() {
        if (DataStorage.LanguageId == 0) {
            return "Yeah,  i  know";
        }
        else if (DataStorage.LanguageId == 1) {
            return "Слышал,  сейчас  займусь";
        }
        else if (DataStorage.LanguageId == 2) {
            return "Чув,  зараз  візьмусь";
        }
        return "";
    }

    public string GetBackText() {
        if (DataStorage.LanguageId == 0) {
            return "Back";
        }
        else if (DataStorage.LanguageId == 1) {
            return "Назад";
        }
        else if (DataStorage.LanguageId == 2) {
            return "Назад";
        }
        return "";
    }

    public string GetCloseText() {
        if (DataStorage.LanguageId == 0) {
            return "Close";
        }
        else if (DataStorage.LanguageId == 1) {
            return "Закрыть";
        }
        else if (DataStorage.LanguageId == 2) {
            return "Закрити";
        }
        return "";
    }

    public string GetMessageText(int step) {
        switch(step) {
            case 0:
                return Message0();
            case 1:
                return Message1();
            case 2:
                return Message2();
            case 3:
                return Message3();
            case 4:
                return Message4();
            case 5:
                return Message5();
            case 6:
                return Message6();
            default:
                return "";
        }
    }

    public string Message0() {
        if (DataStorage.LanguageId == 0) {
            return "New  worker?\nRight  in  time.  Know something  about  our  situation?  Or  need  to  introduce?";
        }
        else if (DataStorage.LanguageId == 1) {
            return "Новый  работник?\nКак  вовремя.  Ты  уже  знаешь  ситуацию,  или  нужно  ввести  в  курс  дела?";
        }
        else if (DataStorage.LanguageId == 2) {
            return "Новий  працівник?\nЯк  вчасно.  Ти  вже  в  курсі  ситуації,  чи  потрібна  допомога?";
        }
         return "";
    }

    public string Message1() {
        if (DataStorage.LanguageId == 0) {
            return "I  have  no  time  for  it. This  blockhead  will explain  you  what  have he  done.";
        }
        else if (DataStorage.LanguageId == 1) {
            return "Мне  некогда  возиться\nс тобой,  вот  этот  оболтус  сейчас  расскажет  тебе  о  работёнке,  которой  он  нам  всем  добавил.";
        }
        else if (DataStorage.LanguageId == 2) {
            return "Мені  ніколи  возитися  з  тобою,  ось  цей телепень  зараз  розповість  тобі  про роботу,  якої  він  нам всім  додав.";
        }
        return "";
    }

    public string Message2() {
        if (DataStorage.LanguageId == 0) {
            return "Hi!  I  am  production manager  at  this factory.  In  another day  I  could  say  this proudly.  But  today  I would  rather  be  a simple  worker.";
        }
        else if (DataStorage.LanguageId == 1) {
            return "Привет!  Я  управляющий  этим  производством.\nВ  другой  день  я  бы  говорил  это  с  гордостью,  но  не  сегодня.  Сегодня  я  предпочёл  бы  быть  рядовым  работником.";
        }
        else if (DataStorage.LanguageId == 2) {
            return "Привіт!  Я  керуючий  цим виробництвом.  В  інший день  я  б  говорив  це  з гордістю,  але  не сьогодні.  Наразі  я  бажав  би  бути  рядовим працівником.";
        }
        return "";
    }

    public string Message3() {
        if (DataStorage.LanguageId == 0) {
            return "All  the  cats  from  our factory  suddenly decided  that  they  had enough  work  and  hid into  the  boxes,  which  they  should  produce. If i  won't  do  something with  it  and  work  will not  resume - dismissal is  trifle  to  what\ni'll  get.";
        }
        else if (DataStorage.LanguageId == 1) {
            return "Все  котики  на  нашей  фабрике  внезапно  решили,  что  хватит  с  них  работы,  и  попрятались  в  коробки,  которые  должны  производить.  Если  я  что-то  с  этим  не  сделаю  и  работа  не  возобновится - увольнением  здесь  я  не  отделаюсь.";
        }
        else if (DataStorage.LanguageId == 2) {
            return "Всі  котики  на  нашій фабриці  раптово вирішили,  що  досить з  них  роботи,  і  поховалися  в  коробки, які  повинні  виробляти. Якщо  з  цим  щось  не  зробити,  звільненням  я  не  відбудусь.";
        }
        return "";
    }

    public string Message4() {
        if (DataStorage.LanguageId == 0) {
            return "I  need  your  help  -  i can't  find  them  all  by myself.  You  need  to  go to  the  workshop  and try  to  find  some  cats. But  remember  that  if you  will  not  find  a pair  to  a  cat - he  will not  move  an  inch.";
        }
        else if (DataStorage.LanguageId == 1) {
            return "Мне  нужна  твоя  помощь - сам  я  всех  не  найду. Тебе  нужно отправиться в  цех  и  попробовать найти  несколько котиков.  Только  учти, что  если  ты  не  найдёшь котику  пару - он  не сдвинется  с  места.";
        }
        else if (DataStorage.LanguageId == 2) {
            return "Мені  потрібна  твоя допомога - сам  я  всіх  не  знайду.  Тобі  потрібно  відправитися в  цех  і  спробувати знайти  кілька  котиків. Тільки  врахуй,  що  якщо  ти  не  знайдеш  котику  пару - він  не  зрушиться з  місця.";
        }
        return "";
    }

    public string Message5() {
        if (DataStorage.LanguageId == 0) {
            return "Hey,  you.  As  i  see  you are  doing  quite  well. Move  to  the  next  workshop.  there  is  an active  conveyor  belt. I  don't  have  enough time  to  search  this loafers.  Find  them  and send  to  their  working places  and  you'll  get  a  reward.";
        }
        else if (DataStorage.LanguageId == 1) {
            return "Эй,  новенький,\nя  смотрю,  ты  неплохо  справляешься.  Отправляйся  в  следующий  цех, там  активная  лента  конвейера,  у  меня  нет времени  искать  этих бездельников.  Найди  их  и  отправь  на  рабочие места,  и  я  отблагодарю тебя.";
        }
        else if (DataStorage.LanguageId == 2) {
            return "Ей,  новенький,\nя  дивлюся,  ти  непогано впорався.  Вирушай  в наступний  цех,  там активна  стрічка  конвеєра,  у  мене  немає часу шукати  цих  нероб.  Знайди  усіx  та  відправ на  робочі  місця,  і  я  віддячу  тобі.";
        }
        return "";
    }

    public string Message6() {
        if (DataStorage.LanguageId == 0) {
            return "Holy  milk!  You're  doing great!  Maybe  i  won't  be fired,  thanks  to  you. Now  you  can  go  to  the hall - there  in  one  of the  boxes  you  can  find Whisker  sleeping.  This mustachioed  old  cat only  can  two  things - sleep  and  judge  others  for  idling.";
        }
        else if (DataStorage.LanguageId == 1) {
            return "Святое  молоко,  ты отлично  справляешься! Может  быть,  благодаря тебе,  меня  еще  и  не уволят. Отправляйся  в  холл,  там  в  одной  из коробок  будет  спать Уискер. Усатый старик  только  и  может  что спать  да  осуждать  других  за  безделие.";
        }
        else if (DataStorage.LanguageId == 2) {
            return "Святе  молоко,  в  тебе  добре  виходить! Можливо,  завдяки  тобі, мене  ще  й  не  звільнять. Вирушай  в  хол,  там  в  одній  з  коробок  спатиме  Уіскер. Вусатий  старий  тільки  і  може,  що  спати  та засуджувати  інших  за  байдикування.";
        }
        return "";
    }

    public string MessageReject() {
        if (DataStorage.LanguageId == 0) {
            return "Are  you  sure?  Well, anyway  i  am  gonna  look after  you.  I  hope  your confidence  is  not unfounded.";
        }
        else if (DataStorage.LanguageId == 1) {
            return "Уверен?  Ну  хорошо,  я  буду  заглядывать  и  следить  за  процессом.  Надеюсь  твоя  уверенность  небезосновательна.";
        }
        else if (DataStorage.LanguageId == 2) {
            return "Впевнений?  Ну  добре,  я буду  стежити  за  процесом.  Сподіваюся, що  твоя  впевненість  небезпідставна.";
        }
        return "";
    }
}
