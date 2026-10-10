using System;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class GameplayManager : SceneManagerBase<GameplayManager>
{
    [SerializeField]
    private int maxRecordCount = 5;

    /// <summary>
    /// ゲーム内スコア    </summary>
    public int Score => score;
    private int score;
    /// <summary>
    /// プレイ時間    </summary>
    private float playTime;
    /// <summary>
    /// プレイ可能状態 </summary>
    private bool isPlaying;
    /// <summary>
    /// ポーズ状態    </summary>
    private bool isPaused;
    /// <summary>
    /// 一度コンティニューしたかどうか    </summary>
    private bool hasContinued;

    private PlayerController player;
    private GameplayUIController gameplayUI;
    private ResultManager resultManager;
    private SaveDataManager saveDataManager;

    private void Start()
    {
        resultManager = ResultManager.Instance;
        saveDataManager = SaveDataManager.Instance;
    }
    private void OnApplicationPause(bool pause)
    {
        if (!isPlaying) return;

        // ポーズ状態の設定
        SetPause(true);
        // ポーズUIの表示・非表示
        if (isPaused) gameplayUI.ShowPauseUI();
        else gameplayUI.HidePauseUI();
    }
    protected override void OnInit()
    {
        player = FindAnyObjectByType<PlayerController>();
        gameplayUI = FindAnyObjectByType<GameplayUIController>();
    }
    protected override void OnStart()
    {
        isPlaying = true;
        player.ChangeState(PlayerState.Alive);
        gameplayUI.Show();
        AudioManager.Instance.PlayBGM("GameplayMain");
    }
    protected override void OnUpdate()
    {
        if (isPaused || !isPlaying) return;

        SetScore((int)player.transform.position.z); // スコア設定・表示更新
        playTime += Time.deltaTime;                 // プレイ時間の計測
    }
    protected override void OnEnd()
    {
        gameplayUI.Hide();
        //セーブデータの更新
        GameProgressData gameProgressData = saveDataManager.GameProgressData;
        int currentHighScore = gameProgressData.highScore;
        bool isUpdateHighScore = this.score > currentHighScore;
        gameProgressData.highScore = Mathf.Max(currentHighScore, score);
        gameProgressData.AddScoreRecord(
            this.score,
            saveDataManager.PlayerData.name,
            maxRecordCount);
        saveDataManager.Save(gameProgressData);
        // GameCenterへのスコア送信
        if(isUpdateHighScore)
        {
            _ = GameCenterManager.Instance.SubmitScore(this.score);
        }
        // リザルト情報を作成しマネージャーに渡す
        ResultData resultData = new ResultData
        {
            score = this.score,
            highScore = gameProgressData.highScore,
            isUpdatedHighScore = isUpdateHighScore,
            playTime = this.playTime,
        };
        resultManager.SetResult(resultData);
        // マネージャーのゲーム終了イベント発火
        AdsManager.Instance.OnGameplayEnded();
    }
    protected override void OnUninit()
    {
        hasContinued = false;
        playTime = 0f;
        AudioManager.Instance.StopBGM();
    }

    /// <summary>
    /// スコア設定    </summary>
    private void SetScore(int value)
    {
        score = value;
        gameplayUI.UpdateScoreText(score); // 表示更新
    }
    /// <summary>
    /// ゲームを一時停止    </summary>
    private void Pause()
    {
        Time.timeScale = 0f;
    }
    /// <summary>
    /// ゲームを再開    </summary>
    private void Resume()
    {
        Time.timeScale = 1f;
    }
    private void FinishGame()
    {
        Debug.Log("スコア:" + score);
        // リザルトへ遷移
        ChangeSceneWithoutLoad(SceneType.Result);
    }
    private void Continue()
    {
        isPlaying = true;
        hasContinued = true;
        player.ChangeState(PlayerState.Revive);
    }

    public void GameOver()
    {
        if (!isPlaying) return;

        isPlaying = false;
        if (hasContinued) FinishGame();
        else gameplayUI.ShowContinueUI();
    }
    /// <summary>
    /// ポーズ状態の切り替え    </summary>
    public void SetPause(bool value)
    {
        isPaused = value;

        if (isPaused) Pause();
        else Resume();
    }
    /// <summary>
    /// コンティニュー画面で"Yes"が選択された    </summary>
    public void OnSelectedContinue()
    {
        // リワード広告の視聴後にコンティニュー
        AdsManager.Instance.SetReward(() => Continue());
        AdsManager.Instance.ShowAd(AdType.Rewarded);
    }
    /// <summary>
    /// コンティニュー画面で"No"が選択された    </summary>
    public void OnSelectedGiveUp()
    {
        FinishGame();
    }
}