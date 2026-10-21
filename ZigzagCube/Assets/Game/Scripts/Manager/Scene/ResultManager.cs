using UnityEngine;

public class ResultManager : SceneManagerBase<ResultManager>
{
    private ResultData resultData;
    private ResultUIController resultUI;
    private AdsManager adsManager;

    protected override void OnInit()
    {
        // リザルトUI表示
        resultUI = FindAnyObjectByType<ResultUIController>();
        resultUI.Show();
        // 広告
        if (adsManager == null) adsManager = AdsManager.Instance;
        adsManager.ShowAd(AdType.Banner);
    }
    protected override void OnStart()
    {
        resultUI.ShowResult(resultData);
        AudioManager.Instance.PlaySE("PopupOpen");
    }
    protected override void OnUninit()
    {
        adsManager.HideAd(AdType.Banner);
    }

    /// <summary>
    /// リザルト情報を設定    </summary>
    public void SetResult(ResultData result) { resultData = result; }
    /// <summary>
    /// タイトルに戻る    </summary>
    public void BackToTile()
    {
        // 広告の表示有無でシーン遷移のアニメーション切り替え
        if (adsManager.CanShowAd(AdType.Interstitial))
        {
            ChangeScene(SceneType.Title, false, "GameplayScene");
        }
        else ChangeScene(SceneType.Title, true, "GameplayScene");
    }
}
