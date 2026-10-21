using UnityEngine;

public class TitleManager : SceneManagerBase<TitleManager>
{
    private TitleUIController titleUI;
    private AdsManager adsManager;

    protected override void OnInit()
    {
        // タイトルUI表示
        titleUI = FindAnyObjectByType<TitleUIController>();
        titleUI.Show();
        // 広告
        if (adsManager == null) adsManager = AdsManager.Instance;
        if (adsManager.CanShowAd(AdType.Interstitial))
        {
            adsManager.ShowAd(AdType.Interstitial);
        }
        adsManager.LoadAd(AdType.Banner);
    }
    protected override void OnStart()
    {
        AudioManager.Instance.PlayBGM("TitleMain");
    }

    /// <summary>
    /// ゲーム開始    </summary>
    public void StartGame()
    {
        ChangeSceneWithoutLoad(SceneType.Gameplay);
    }
}
