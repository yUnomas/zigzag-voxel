using System;
using GoogleMobileAds.Api;
using UnityEngine;

public class InterstitialAdHandler : MonoBehaviour
{
    private InterstitialAd interstitialAd;

    private void Destroy()
    {
        if (interstitialAd == null) return;

        interstitialAd.Destroy();
        interstitialAd = null;
    }
    private void RegisterAdEvents()
    {
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.LogWarning($"インタースティシャル広告の表示開始");
            AudioListener.pause = true;
        };
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.LogWarning($"インタースティシャル広告の表示終了");
            AudioListener.pause = false;
            Load();
        };
        interstitialAd.OnAdFullScreenContentFailed += error =>
        {
            Debug.LogWarning($"インタースティシャル広告の表示失敗\n{error}");
            AudioListener.pause = false;
            Load();
        };
    }

    public void Load()
    {
        Destroy();
#if UNITY_IOS || UNITY_ANDROID
        // インタースティシャル広告のリクエスト送信
        var request = new AdRequest();
        InterstitialAd.Load(AdsManager.Instance.Config.InterstitialAdUnitId, request, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                Debug.LogWarning($"インタースティシャル広告の読み込み失敗\n{error}");
                return;
            }
            if (ad == null)
            {
                Debug.LogWarning("インタースティシャル広告の取得失敗");
                return;
            }

            Debug.Log("インタースティシャル広告の読み込み完了");
            interstitialAd = ad;
            RegisterAdEvents();
        });
#endif
    }
    public void Show()
    {
        if (interstitialAd == null || !interstitialAd.CanShowAd())
        {
            Debug.Log("インタースティシャル広告の表示不可");
            return;
        }
        interstitialAd.Show();
    }
}
