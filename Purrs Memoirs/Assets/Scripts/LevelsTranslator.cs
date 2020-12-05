using UnityEngine;
using UnityEngine.UI;

public class LevelsTranslator : MonoBehaviour {

    public Text selectLevelText;

    private void Start() {
        selectLevelText.text = LanguageTitles.GetInstance().selectLevelTitle;
    }
}
