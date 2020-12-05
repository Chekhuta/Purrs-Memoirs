using UnityEngine.SceneManagement;
using UnityEngine;
using Firebase.Analytics;

public class GDPRPopup : MonoBehaviour {

    public GameObject pauseBg;
    public GameObject popup;
    public GameObject collectData;
    public GameObject lessRelevant;

    public void ShowCollectData() {
        popup.SetActive(false);
        collectData.SetActive(true);
    }

    public void ShowLessRelevant() {
        popup.SetActive(false);
        lessRelevant.SetActive(true);
    }

    public void ShowPopup() {
        pauseBg.SetActive(true);
        popup.SetActive(true);
    }

    public void CloseLessRelevant() {
        lessRelevant.SetActive(false);
        popup.SetActive(true);
    }

    public void CloseCollectData() {
        collectData.SetActive(false);
        popup.SetActive(true);
    }

    public void AgreeConsent() {
        gameObject.SetActive(false);
        DataStorage.SetNPA(0);
        FirebaseAnalytics.SetUserProperty(FirebaseAnalytics.UserPropertyAllowAdPersonalizationSignals, "true");
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        if (SceneManager.GetActiveScene().name == "Logo") {
            FindObjectOfType<Logo>().LoadMenuScene();
        }
        else if (SceneManager.GetActiveScene().name == "Menu") {
            FindObjectOfType<Menu>().SetActiveButtons(true);
        }
    }

    public void RejectConsent() {
        lessRelevant.SetActive(false);
        popup.SetActive(true);
        gameObject.SetActive(false);
        DataStorage.SetNPA(1);
        FirebaseAnalytics.SetUserProperty(FirebaseAnalytics.UserPropertyAllowAdPersonalizationSignals, "false");
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(false);
        if (SceneManager.GetActiveScene().name == "Logo") {
            FindObjectOfType<Logo>().LoadMenuScene();
        }
        else if (SceneManager.GetActiveScene().name == "Menu") {
            FindObjectOfType<Menu>().SetActiveButtons(true);
        }
    }

    public void OpenPrivacyPolicy() {
        Application.OpenURL("");
    }

    public void OpenPartnerPrivacyPolicy() {
        Application.OpenURL("");
    }
}
