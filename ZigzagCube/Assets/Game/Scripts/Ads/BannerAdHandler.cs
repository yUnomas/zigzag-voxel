using GoogleMobileAds.Api;
using UnityEngine;

public class BannerAdHandler : MonoBehaviour
{
    private BannerView bannerView;
    private AdSize adaptiveSize;

    private void Awake()
    {
#if UNITY_IOS || UNITY_ANDROID
        /// 端末に適した広告サイズを取得
        int deviceWidth = MobileAds.Utils.GetDeviceSafeWidth();
        adaptiveSize =
            AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(deviceWidth);
#endif
    }

    private void Destroy()
    {
        if (bannerView == null) return;

        bannerView.Destroy();
        bannerView = null;
    }
    private void RegisterAdEvents()
    {
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("バナー広告の読み込み完了");
        };
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.Log($"バナー広告の読み込み失敗 Error:{error}");
        };
        bannerView.OnAdClicked += () =>
        {
            Debug.Log("バナー広告のクリック");
        };
    }

    public void Load()
    {
        Destroy();
#if UNITY_IOS || UNITY_ANDROID
        bannerView = new BannerView(AdsManager.Instance.Config.BannerAdUnitId, adaptiveSize, AdPosition.Top);
        RegisterAdEvents();
        bannerView.Hide();
        bannerView.LoadAd(new AdRequest());
#endif
    }
    public void Show()
    {
        bannerView?.Show();
    }
    public void Hide()
    {
        bannerView?.Hide();
    }
}