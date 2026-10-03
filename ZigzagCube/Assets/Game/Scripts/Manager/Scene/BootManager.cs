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

        SaveDataManager saveDataManager = SaveDataManager.Instance;
        saveDataManager.LoadAll();
        AudioManager.Instance.ApplySettings(saveDataManager.SettingsData);
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
#endif
        // ゲーム開始時に遷移するシーンを設定
        ChangeScene(SceneType.Title, true, "GameplayScene");
    }
}