using UnityEngine;

public class ResultManager : SceneManagerBase<ResultManager>
{
    private ResultData resultData;
    private ResultUIController resultUI;

    protected override void OnInit()
    {
        resultUI = FindAnyObjectByType<ResultUIController>();
        resultUI.Show();
        AdsManager.Instance.ShowAd(AdType.Banner);
    }
    protected override void OnStart()
    {
        resultUI.ShowResult(resultData);
        AudioManager.Instance.PlaySE("PopupOpen");
    }
    protected override void OnUninit()
    {
        AdsManager.Instance.HideAd(AdType.Banner);
    }

    /// <summary>
    /// リザルト情報を設定    </summary>
    public void SetResult(ResultData result) { resultData = result; }
    /// <summary>
    /// タイトルに戻る    </summary>
    public void BackToTile()
    {
        // 広告の表示有無でシーン遷移のアニメーション切り替え
        if (AdsManager.Instance.IsInterstitialTiming())
        {
            ChangeScene(SceneType.Title, false, "GameplayScene");
        }
        else ChangeScene(SceneType.Title, true, "GameplayScene");
    }
}
