using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Firebase.Analytics;

public class ConsentCheck : MonoBehaviour {

    private bool isEEA;

    public void CheckGDPROnLogo(Logo logo) {
        StartCoroutine(CheckEEAOnLogo(logo));
    }

    private IEnumerator CheckEEAOnLogo(Logo logo) {
        UnityWebRequest request = UnityWebRequest.Get("");
        yield return request.SendWebRequest();

        if (request.isDone) {
            EEA eea = JsonUtility.FromJson<EEA>(request.downloadHandler.text);
            isEEA = eea.is_request_in_eea_or_unknown;
            DataStorage.IsEEA = isEEA;

            if (isEEA) {
                if (DataStorage.NPA == -2 || DataStorage.NPA == -1) {
                    logo.ShowGDPRPopup();
                }
            }
            else {
                if (DataStorage.NPA == -2) {
                    DataStorage.SetNPA(-1);
                    FirebaseAnalytics.SetUserProperty(FirebaseAnalytics.UserPropertyAllowAdPersonalizationSignals, "true");
                    FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                }
            }
        yield return null;
        }
    }
}

class EEA {
    public bool is_request_in_eea_or_unknown;
}
