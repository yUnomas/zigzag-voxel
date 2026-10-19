using GoogleMobileAds.Api;
using UnityEngine;

public class InterstitialAdHandler : MonoBehaviour
{
    private InterstitialAd interstitialAd;
    private bool isLoading;

    private void OnDestroy()
    {
        Destroy();
    }

    private void Destroy()
    {
        interstitialAd?.Destroy();
        interstitialAd = null;
    }
    private void RegisterAdEvents()
    {
        interstitialAd.OnAdFullScreenContentOpened += async () =>
        {
            await Awaitable.MainThreadAsync();

            Debug.Log("インタースティシャル広告の表示開始");
            AudioListener.pause = true;
        };

        interstitialAd.OnAdFullScreenContentClosed += async () =>
        {
            await Awaitable.MainThreadAsync();

            Debug.Log("インタースティシャル広告の表示終了");
            AudioListener.pause = false;
            Load();
        };

        interstitialAd.OnAdFullScreenContentFailed += async error =>
        {
            await Awaitable.MainThreadAsync();

            Debug.LogWarning($"インタースティシャル広告の表示失敗\n{error}");
            AudioListener.pause = false;
            Load();
        };
    }

    public void Load()
    {
        if (isLoading) return;

        isLoading = true;
        Destroy();

#if UNITY_IOS || UNITY_ANDROID
        InterstitialAd.Load(
            AdsManager.Instance.Config.InterstitialAdUnitId,
            new AdRequest(),
            async (InterstitialAd ad, LoadAdError error) =>
            {
                await Awaitable.MainThreadAsync();

                isLoading = false;

                if (error != null || ad == null)
                {
                    Debug.LogWarning(
                        $"インタースティシャル広告の読み込み失敗\n{error}");
                    ad?.Destroy();
                    return;
                }

                Debug.Log("インタースティシャル広告の読み込み完了");
                interstitialAd = ad;
                RegisterAdEvents();
            });
#else
        isLoading = false;
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