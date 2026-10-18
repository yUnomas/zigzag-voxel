using System;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    [Header("=====")]
    [SerializeField] private AdConfig config;
    public AdConfig Config => config;
    [SerializeField] private InterstitialAdHandler interstitialAdHandler;
    [SerializeField] private RewardedAdHandler rewardedAdHandler;
    [SerializeField] private BannerAdHandler bannerAdHandler;
    [SerializeField] private UMPManager umpManager;

    public static AdsManager Instance => instance;
    private static AdsManager instance;

    /// <summary>
    /// プレイ回数    </summary>
    private int sessionPlayCount;

    private void Awake()
    {
        // インスタンス化
        if (instance == null) instance = this;
    }
    private void Start()
    {
        umpManager.RequestConsent(canRequestAds =>
        {
            if(!canRequestAds)
            {
                Debug.LogWarning("Ads cannot be requested.");
                return;
            }

            Initialize();
        });
    }

    /// <summary>
    /// 初期化    </summary>
    private void Initialize()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            if (initStatus == null)
            {
                Debug.LogError("Google Mobile Ads cannot be initialized");
                return;
            }

            Debug.Log("Google Mobile Ads initialized");
            interstitialAdHandler.Load();
            rewardedAdHandler.Load();
            bannerAdHandler.Load();
        });
    }
    /// <summary>
    /// 指定広告のロード    </summary>
    public void LoadAd(AdType type)
    {
        switch (type)
        {
            case AdType.Interstitial: interstitialAdHandler.Load(); break;
            case AdType.Rewarded: rewardedAdHandler.Load(); break;
            case AdType.Banner: bannerAdHandler.Load(); break;
        }
    }
    /// <summary>
    /// 広告表示    </summary>
    public void ShowAd(AdType type)
    {
        switch (type)
        {
            case AdType.Interstitial: interstitialAdHandler.Show(); break;
            case AdType.Rewarded: rewardedAdHandler.Show(); break;
            case AdType.Banner: bannerAdHandler.Show(); break;
        }
    }
    /// <summary>
    /// 表示可能な場合のみ広告を表示    </summary>
    public void TryShowAd(AdType type)
    {
        switch (type)
        {
            case AdType.Interstitial:
                {
                    if(IsInterstitialTiming()) interstitialAdHandler.Show();
                }
                break;
        }
    }
    public void HideAd(AdType type)
    {
        switch (type)
        {
            case AdType.Banner: bannerAdHandler.Hide(); break;
        }
    }

    /// <summary>
    /// インタースティシャル広告の表示タイミングか判定    </summary>
    /// <returns>
    /// セッション中の1プレイ目、または一定の表示間隔に達した場合は true </returns>
    public bool IsInterstitialTiming()
    {
        return sessionPlayCount == 1 || (sessionPlayCount - 1) % Config.interstitialAdDisplayInterval == 0;
    }
    /// <summary>
    /// リワード広告の報酬を設定    </summary>
    public void SetReward(Action action)
    {
        rewardedAdHandler.SetReward(action);
    }

    /// <summary>
    /// ゲームプレイ終了時のイベント    </summary>
    public void OnGameplayEnded()
    {
        sessionPlayCount++;
    }
}