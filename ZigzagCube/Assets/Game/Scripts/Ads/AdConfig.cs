using UnityEngine;

[CreateAssetMenu(fileName="AdConfig")]
public class AdConfig : ScriptableObject
{
    [SerializeField, Tooltip("本番広告IDの使用切り替え")]
    private bool useProductionAds = false;
    [Header("Banner")]
    [SerializeField, Tooltip("iOS用本番バナー広告ID")]
    private string bannerProductionAdUnitIdForIOS = string.Empty;
    [SerializeField, Tooltip("Android用本番バナー広告ID")]
    private string bannerProductionAdUnitIdForAndroid = string.Empty;
    [Header("Interstitial")]
    [SerializeField, Tooltip("インタースティシャル広告の表示間隔")]
    public int interstitialAdDisplayInterval = 3;
    [SerializeField, Tooltip("iOS用本番インタースティシャル広告ID")] 
    private string interstitialProductionAdUnitIdForIOS = string.Empty;
    [SerializeField, Tooltip("Android用本番インタースティシャル広告ID")]
    private string interstitialProductionAdUnitIdForAndroid = string.Empty;
    [Header("Rewarded")]
    [SerializeField, Tooltip("iOS用本番リワード広告ID")] 
    private string rewardedProductionAdUnitIdForIOS = string.Empty;
    [SerializeField, Tooltip("Android用本番リワード広告ID")] 
    private string rewardedProductionAdUnitIdForAndroid = string.Empty;

#if UNITY_IOS
    public string BannerAdUnitId =>
        useProductionAds ? bannerProductionAdUnitIdForIOS : "ca-app-pub-3940256099942544/2934735716";
#elif UNITY_ANDROID
    public string BannerAdUnitId =>
        useProductionAds ? bannerProductionAdUnitIdForAndroid : "ca-app-pub-3940256099942544/6300978111";
#else
    public string BannerAdUnitId = "unused";
#endif

#if UNITY_IOS
    public string InterstitialAdUnitId =>
        useProductionAds ? interstitialProductionAdUnitIdForIOS : "ca-app-pub-3940256099942544/4411468910";
#elif UNITY_ANDROID
    public string InterstitialAdUnitId =>
        useProductionAds ? interstitialProductionAdUnitIdForAndroid : "ca-app-pub-3940256099942544/1033173712";
#else
    public string InterstitialAdUnitId = "unused";
#endif

#if UNITY_IOS
    public string RewardedAdUnitId =>
        useProductionAds ? rewardedProductionAdUnitIdForIOS : "ca-app-pub-3940256099942544/1712485313";
#elif UNITY_ANDROID
    public string RewardedAdUnitId =>
        useProductionAds ? rewardedProductionAdUnitIdForAndroid : "ca-app-pub-3940256099942544/5224354917";
#else
    public string RewardedAdUnitId = "unused";
#endif
}