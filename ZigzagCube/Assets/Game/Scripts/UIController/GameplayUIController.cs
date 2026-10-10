using System;
using TMPro;
using UnityEngine;

public class GameplayUIController : UIControllerBase
{
    [SerializeField] private TextMeshProUGUI scoreTMP;
    [SerializeField] private PauseUIController pauseUI;
    [SerializeField] private ContinueUIController continueUI;
    [SerializeField] private CountdownUIController countdownUI;

    public void ShowContinueUI() { continueUI.Show(); }
    public void ShowPauseUI() { pauseUI.Show(); }
    public void HidePauseUI() { pauseUI.Hide(); }

    /// <summary>
    /// スコアの表示更新    </summary>
    /// <param name="score">
    /// 現在のスコア  </param>
    public void UpdateScoreText(int score)
    {
        scoreTMP.text = $"{score}";
    }
    /// <summary>
    /// ゲーム再開までのカウントダウン開始    </summary>
    /// <param name="seconds">
    /// カウントダウン秒数    </param>
    /// <param name="onCompleted">
    /// カウントダウン終了後に実行するイベント    </param>
    public void StartCountDown(int seconds, Action onCompleted)
    {
        countdownUI.Countdown(seconds, onCompleted);
    }

    /// <summary>
    /// ポーズボタンが押された際のイベント    </summary>
    public void OnClickPause()
    {
        GameplayManager.Instance.SetPause(true);
        ShowPauseUI();
    }
    /// <summary>
    ///ポーズパネルが押された際のイベント     </summary>
    public void OnClickPausePanel()
    {
        GameplayManager.Instance.SetPause(false);
        HidePauseUI();
    }
}
