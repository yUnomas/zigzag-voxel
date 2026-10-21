using GoogleMobileAds.Api;
using Unity.VisualScripting;
using UnityEngine;

public class BannerAdHandler : MonoBehaviour
{
    private BannerView bannerView;
    private AdSize adaptiveSize;
    private string unitId;

    private void OnDestroy()
    {
        Destroy();
    }

    private void Destroy()
    {
        bannerView?.Destroy();
        bannerView = null;
    }
    private void RegisterAdEvents()
    {
        bannerView.OnBannerAdLoaded += async () =>
        {
            await Awaitable.MainThreadAsync();

            Debug.Log("バナー広告の読み込み完了");
        };
        bannerView.OnBannerAdLoadFailed += async (LoadAdError error) =>
        {
            await Awaitable.MainThreadAsync();

            Debug.Log($"バナー広告の読み込み失敗 Error:{error}");
        };
        bannerView.OnAdClicked += async () =>
        {
            await Awaitable.MainThreadAsync();

            Debug.Log("バナー広告のクリック");
        };
    }

    public void Initialize(AdConfig config)
    {
        if (config != null) unitId = config.BannerAdUnitId;
#if UNITY_IOS || UNITY_ANDROID
        /// 端末に適した広告サイズを取得
        int deviceWidth = MobileAds.Utils.GetDeviceSafeWidth();
        adaptiveSize =
            AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(deviceWidth);

        Load();
#endif
    }
    public void Load()
    {
        Destroy();

#if UNITY_IOS || UNITY_ANDROID
        bannerView = new BannerView(unitId, adaptiveSize, AdPosition.Top);
        if(bannerView == null)
        {
            Debug.LogWarning("バナー広告の生成失敗");
            return;
        }

        Debug.LogWarning("バナー広告の生成成功");
        RegisterAdEvents();
        bannerView.Hide();
        bannerView.LoadAd(new AdRequest());
#endif
    }
    public bool CanShow()
    {
        if(bannerView == null)
        {
            Debug.LogWarning("バナー広告の表示不可");
            return false;
        }
        return true;
    }
    public void Show()
    {
        if (!CanShow()) return;

        bannerView.Show();
    }
    public void Hide()
    {
        bannerView?.Hide();
    }
}