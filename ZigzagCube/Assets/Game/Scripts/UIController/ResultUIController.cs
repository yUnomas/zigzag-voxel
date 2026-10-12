using System;
using TMPro;
using UnityEngine;

public class ResultUIController : UIControllerBase
{
    [SerializeField] private TextMeshProUGUI newRecordTMP;
    [SerializeField] private TextMeshProUGUI scoreTMP;
    [SerializeField] private TextMeshProUGUI highScoreTMP;
    [SerializeField] private TextMeshProUGUI playTimeTMP;
    [SerializeField] private TextMeshProUGUI turnsTMP;
    [SerializeField] private TextMeshProUGUI levelTMP;

    private ResultData resultData;

    /// <summary>
    /// プレイ時間の表示更新    </summary>
    private void UpdatePlayTime(float playTime)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(playTime);
        // 1時間を超えていたら、「時:分:秒」で表示
        if (timeSpan.TotalHours >= 1)
        {
            playTimeTMP.text = $"{(int)timeSpan.TotalHours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }
        // 「分:秒」で表示
        else
        {
            playTimeTMP.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }
    }

    /// <summary>
    /// リザルト表示    </summary>
    public void ShowResult(ResultData result)
    {
        resultData = result;

        // 数値の表示更新
        scoreTMP.text = result.score.ToString();
        highScoreTMP.text = $"{result.highScore}";
        turnsTMP.text = $"{result.turns}";
        levelTMP.text = $"{result.level}";
        UpdatePlayTime(result.playTime);
        // ハイスコアの更新状況によって、新記録ラベルを表示
        if (result.isUpdatedHighScore)
        {
            newRecordTMP.enabled = true;
            AudioManager.Instance.PlaySE("NewRecord");
        }
    }
    /// <summary>
    /// （タイトルへ）戻るボタンが押された際のイベント    </summary>
    public void OnClickReturn()
    {
        ResultManager.Instance.BackToTile();
    }
    /// <summary>
    /// 共有ボタンが押された際のイベント    </summary>
    public void OnClickShare()
    {
        GetComponent<SocialShareModule>().ShareResult(resultData);
    }
}
