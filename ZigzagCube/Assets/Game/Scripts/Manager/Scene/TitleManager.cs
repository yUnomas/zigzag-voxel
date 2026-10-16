using UnityEngine;

public class TitleManager : SceneManagerBase<TitleManager>
{
    private TitleUIController titleUI;

    protected override void OnInit()
    {
        // タイトルUI表示
        titleUI = FindAnyObjectByType<TitleUIController>();
        titleUI.Show();
        // 広告
        AdsManager.Instance.TryShowAd(AdType.Interstitial);
        AdsManager.Instance.LoadAd(AdType.Banner);
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
