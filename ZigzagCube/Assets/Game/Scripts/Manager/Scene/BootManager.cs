using UnityEngine;

public class BootManager : SceneManagerBase<BootManager>
{
    [SerializeField, Tooltip("デバッグ開始するシーンの種類")]
    private SceneType debugStartSceneType = SceneType.Boot;

    protected override void OnInit()
    {
        // 初期設定: 本来は設定ファイルのロード処理で行うべき
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        AudioManager.Instance.ApplySettings(SaveDataManager.Instance.SettingsData);
    }
    protected override void OnStart()
    {
#if UNITY_EDITOR
        // デバッグ開始
        if (debugStartSceneType != SceneType.None && debugStartSceneType != SceneType.Boot)
        {
            ChangeScene(debugStartSceneType, false, "GameplayScene");
            return;
        }
        else StartTitle();
#elif UNITY_IOS
        if (AdsManager.Instance.IsConsentCompleted)
        {
            StartTitle();
            return;
        }

        AdsManager.Instance.OnConsentCompleted += StartTitle;
#else
        StartTitle();
#endif
    }

    private void StartTitle()
    {
#if UNITY_IOS && !UNITY_EDITOR
    AdsManager.Instance.OnConsentCompleted -= StartTitle;
#endif
        ChangeScene(SceneType.Title, true, "GameplayScene");
    }
}