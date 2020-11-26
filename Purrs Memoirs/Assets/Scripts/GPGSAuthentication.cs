using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GPGSAuthentication : MonoBehaviour {

    public static PlayGamesPlatform platform;

    public static void InitializeGPGS() {
        if (platform == null) {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);

            platform = PlayGamesPlatform.Activate();
        }
        if (!Social.localUser.authenticated) {
            Social.localUser.Authenticate(success => { });
        }
    }

    public static void SignOut() {
        PlayGamesPlatform.Instance.SignOut();
    }

    public static bool IsInitialize() {
        return Social.localUser.authenticated;
    }
}
