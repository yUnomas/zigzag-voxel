using System;
using GoogleMobileAds.Api;
using UnityEngine;

public class RewardedAdHandler : MonoBehaviour
{
    private RewardedAd rewardedAd;
    private Action onRewarded;
    
    private bool isRewardEarned;
    private bool isAdClosed;
    private bool isLoading;

    private void EarnReward()
    {
        if (!isRewardEarned || !isAdClosed) return;

        isRewardEarned = false;
        isAdClosed = false;

        Action reward = onRewarded;
        onRewarded = null;
        reward?.Invoke();
    }
    private void Destroy()
    {
        rewardedAd?.Destroy();
        rewardedAd = null;
    }
    private void RegisterAdEvents()
    {
        rewardedAd.OnAdFullScreenContentOpened += async () =>
        {
            await Awaitable.MainThreadAsync();

            Debug.Log($"リワード広告の表示開始");
            AudioListener.pause = true;
        };
        rewardedAd.OnAdFullScreenContentClosed += async () =>
        {
            await Awaitable.MainThreadAsync();

            Debug.Log($"リワード広告の表示終了");
            AudioListener.pause = false;

            isAdClosed = true;
            EarnReward();

            Load();
        };
        rewardedAd.OnAdFullScreenContentFailed += async error =>
        {
            await Awaitable.MainThreadAsync();

            Debug.LogWarning($"リワード広告の表示失敗\n{error}");
            AudioListener.pause = false;

            onRewarded = null;
            isRewardEarned = false;
            isAdClosed = false;

            Load();
        };
    }

    public void Load()
    {
        if(isLoading) return;

        isLoading = true;
        Destroy();

#if UNITY_IOS || UNITY_ANDROID
        // リワード広告のリクエスト送信
        RewardedAd.Load(AdsManager.Instance.Config.RewardedAdUnitId, new AdRequest(), async (RewardedAd ad, LoadAdError error) =>
        {
            await Awaitable.MainThreadAsync();

            isLoading = false;

            if (error != null || ad == null)
            {
                Debug.LogWarning($"リワード広告の読み込み失敗\n{error}");
                ad?.Destroy();
                return;
            }
            
            Debug.Log("リワード広告の読み込み完了");
            rewardedAd = ad;
            RegisterAdEvents();
        });
#else
        isLoading = false;
#endif
    }
    public void Show()
    {
        if (rewardedAd == null || !rewardedAd.CanShowAd())
        {
            Debug.LogWarning("リワード広告の表示不可");
            return;
        }

        isRewardEarned = false;
        isAdClosed = false;
        rewardedAd.Show(async (Reward reward) =>
        {
            await Awaitable.MainThreadAsync();

            isRewardEarned = true;
            EarnReward();
        });
    }
    public void SetReward(Action action)
    {
        onRewarded = action;
    }
}