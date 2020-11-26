using UnityEngine;
using UnityEngine.UI;

public class AreasTranslator : MonoBehaviour {

    public Text selectAreaText;
    public Text Area1;
    public Text Area2;
    public Text Area3;
    public Text Soon1;
    public Text Soon2;

    private void Start() {
        LanguageTitles languageTitles = LanguageTitles.GetInstance();
        selectAreaText.text = languageTitles.selectAreaTitle;
        Area1.text = languageTitles.areaTitle + "  1";
        Area2.text = languageTitles.areaTitle + "  2";
        Area3.text = languageTitles.areaTitle + "  3";
        Soon1.text = languageTitles.soonTitle;
        Soon2.text = languageTitles.soonTitle;
    }

}
