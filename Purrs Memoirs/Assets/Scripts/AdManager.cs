using UnityEngine;
using System;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour {

    public bool IsInGame { get; set; } = false;
    private static bool mobileAdsInit = false;
    private string interstitialID = "";
    private string rewardVideoID = "";
    private bool internetConnection = true;
    private InterstitialAd interstitialAd;
    private RewardBasedVideoAd rewardVideoAd;
    private float interstitialAdShowingDelay = 1;
    private float timeDelay = 0;
    private int timeAttackGamePlayed = 0;
    private LevelCompletePanel levelCompletePanel;

    private void Awake() {
        if (mobileAdsInit) {
            Destroy(gameObject);
        }
        else {
            mobileAdsInit = true;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() {

        if (DataStorage.NPA == -2) {
            return;
        }

        rewardVideoAd = RewardBasedVideoAd.Instance;

        rewardVideoAd.OnAdLoaded += HandleRewardBasedVideoLoaded;
        rewardVideoAd.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        rewardVideoAd.OnAdRewarded += HandleRewardBasedVideoRewarded;
        rewardVideoAd.OnAdClosed += HandleRewardBasedVideoClosed;
        RequestInterstitial();
        RequestRewardVideo();
    }

    private void Update() {
        if (IsInGame) {
            timeDelay += Time.deltaTime;
        }
    }

    private void RequestInterstitial() {
        interstitialAd = new InterstitialAd(interstitialID);
        interstitialAd.OnAdLoaded += HandleInterstitialOnAdLoaded;
        interstitialAd.OnAdFailedToLoad += HandleInterstitialOnAdFailedToLoad;
        interstitialAd.OnAdClosed += HandleInterstitialOnAdClosed;

        AdRequest request = null;
        if (DataStorage.NPA == 1) {
            request = new AdRequest.Builder().AddExtra("npa", "1").Build();
        }
        else if (DataStorage.NPA == -1 || DataStorage.NPA == 0) {
            request = new AdRequest.Builder().Build();
        }

        interstitialAd.LoadAd(request);
    }

    public void RequestRewardVideo() {

        AdRequest request = null;
        if (DataStorage.NPA == 1) {
            request = new AdRequest.Builder().AddExtra("npa", "1").Build();
        }
        else if (DataStorage.NPA == -1 || DataStorage.NPA == 0) {
            request = new AdRequest.Builder().Build();
        }

        rewardVideoAd.LoadAd(request, rewardVideoID);
    }

    public void CheckAdLoad() {
        if (interstitialAd == null) {
            return;
        }
        if (!internetConnection && !interstitialAd.IsLoaded()) {
            if (!interstitialAd.IsLoaded()) {
                RequestInterstitial();
            }
            if (!rewardVideoAd.IsLoaded()) {
                RequestRewardVideo();
            }
        }
    }

    public void DisplayInterstitial() {
        if (DataStorage.NPA == -2) {
            return;
        }
        if (Game.GameMode != 4) {
            if (interstitialAdShowingDelay > 3 || Mathf.Approximately(interstitialAdShowingDelay, 3) || (timeDelay > 180 && interstitialAdShowingDelay > 2)) {
                if (interstitialAd.IsLoaded()) {
                    timeDelay = 0;
                    interstitialAdShowingDelay = 0;
                    interstitialAd.Show();
                }
            }
        }
        else {
            if (timeAttackGamePlayed >= 3) {
                if (interstitialAd.IsLoaded()) {
                    timeAttackGamePlayed = 0;
                    interstitialAd.Show();
                }
            }
        }
    }

    public void DisplayRewardVideo(LevelCompletePanel panel) {
        if (DataStorage.NPA == -2) {
            return;
        }
        levelCompletePanel = panel;
        if (rewardVideoAd.IsLoaded()) {
            rewardVideoAd.Show();
        }
    }

    public bool IsLoadRewardVideo() {
        if (rewardVideoAd != null) {
            return rewardVideoAd.IsLoaded();
        }
        else {
            return false;
        }
    }

    public void IncreaseDelay(float delay) {
        IsInGame = false;
        interstitialAdShowingDelay += delay;
    }

    public void IncreaseAttackTimeGamePlayed() {
        timeAttackGamePlayed++;
    }

    public void HandleInterstitialOnAdLoaded(object sender, EventArgs args) {
        internetConnection = true;
    }


    public void HandleInterstitialOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
        if (args.Message == "Network Error") {
            internetConnection = false;
        }
        else {
            RequestInterstitial();
        }
    }

    public void HandleInterstitialOnAdClosed(object sender, EventArgs args) {
        interstitialAd.OnAdLoaded -= HandleInterstitialOnAdLoaded;
        interstitialAd.OnAdFailedToLoad -= HandleInterstitialOnAdFailedToLoad;
        interstitialAd.OnAdClosed -= HandleInterstitialOnAdClosed;
        interstitialAd.Destroy();
        RequestInterstitial();
    }

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args) {
        internetConnection = true;
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
        if (args.Message == "Network Error") {
            internetConnection = false;
        }
        else {
            RequestRewardVideo();
        }
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args) {
        RequestRewardVideo();
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args) {
        levelCompletePanel.ContinueGame();
    }

}
