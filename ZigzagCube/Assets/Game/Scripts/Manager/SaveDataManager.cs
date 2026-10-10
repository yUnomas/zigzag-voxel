using System.IO;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    private static SaveDataManager instance;
    public static SaveDataManager Instance => instance;

    /// <summary>
    /// 設定データ    </summary>
    public SettingsData SettingsData => settingsData;
    private SettingsData settingsData;
    /// <summary>
    /// ゲーム進行データ    </summary>
    public GameProgressData GameProgressData => gameProgressData;
    private GameProgressData gameProgressData;
    /// <summary>
    /// プレイヤーデータ    </summary>
    public PlayerData PlayerData => playerData;
    private PlayerData playerData;

    //** 各データの保存パス・キー
    // 設定データ
    private const string SettingsDataFilePath = "settings.json";
    private const string SettingsDataKey = "SettingsData";
    // ゲーム進行データ
    private const string GameProgressDataFilePath = "game.json";
    private const string GameProgressDataKey = "GameProgressData";
    // プレイヤーデータ
    private const string PlayerDataFilePath = "player.json";
    private const string PlayerDataKey = "PlayerData";

    private void Awake()
    {
        // インスタンス化
        if (instance == null)
        {
            instance = this;
            LoadAll();
        }

    }
    private void OnApplicationQuit()
    {
        SaveALL();
    }

    /// <summary>
    /// ファイルパスを取得( JSON用 )    </summary>
    private string GetFilePath<T>()
    {
        if(typeof(T) == typeof(SettingsData)) return SettingsDataFilePath;
        if(typeof(T) == typeof(GameProgressData)) return GameProgressDataFilePath;
        if(typeof(T) == typeof(PlayerData)) return PlayerDataFilePath;
        return string.Empty;
    }
    /// <summary>
    /// ファイルパスを取得( PlayerPrefs用 )   </summary>
    private string GetKey<T>()
    {
        if (typeof(T) == typeof(SettingsData)) return SettingsDataKey;
        if (typeof(T) == typeof(GameProgressData)) return GameProgressDataKey;
        if (typeof(T) == typeof(PlayerData)) return PlayerDataKey;
        return string.Empty;
    }

    /// <summary>
    /// 新規セーブデータ作成    </summary>
    public void CreateNewSaveData()
    {
        // ゲーム進行データだけ初期化
        gameProgressData = new GameProgressData();
        Save<GameProgressData>(gameProgressData);
    }
    /// <summary>
    /// データの取得    </summary>
    public T Get<T>() where T : class
    {
        if (typeof(T) == typeof(SettingsData)) return settingsData as T;
        if (typeof(T) == typeof(GameProgressData)) return gameProgressData as T;
        return null;
    }
    /// <summary>
    /// データの保存    </summary>
    public void Save<T>(T data)
    {
        if (data == null) return;

        string key = GetKey<T>();
        string filePath = GetFilePath<T>();

        // ゲーム進行データをJSON変換し、格納(全プラットフォーム共通)
        string json = JsonUtility.ToJson(data);

#if UNITY_WEBGL && !UNITY_EDITOR
        //** WebGLビルド時
        // JSONをそのままPlayerPrefsに保存
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
#else
        //** PC向けビルド・エディタ実行時
        // persistentDataPathにjsonファイルとして保存
        string path = Path.Combine(Application.persistentDataPath, filePath);
        File.WriteAllText(path, json);
#endif
    }
    /// <summary>
    /// データの読み込み    </summary>
    public T Load<T>() where T : new()
    {
        string key = GetKey<T>();
        string filePath = GetFilePath<T>();
        string json = "";

#if UNITY_WEBGL && !UNITY_EDITOR
        // WebGLからのロード
        if (PlayerPrefs.HasKey(key))
        {
            json = PlayerPrefs.GetString(key);
        }
#else
        // PC/エディタからのロード
        string path = Path.Combine(Application.persistentDataPath, filePath);
        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
        }
#endif

        // JSONが空でなければクラスに復元し、空なら新規作成
        if (!string.IsNullOrEmpty(json)) return JsonUtility.FromJson<T>(json);
        else return new T();
    }
    /// <summary>
    /// 全データのセーブ    </summary>
    public void SaveALL()
    {
        Save<SettingsData>(settingsData);
        Save<GameProgressData>(gameProgressData);
        Save<PlayerData>(playerData);
    }
    /// <summary>
    /// 全データのロード    </summary>
    public void LoadAll()
    {
        settingsData = Load<SettingsData>();
        gameProgressData = Load<GameProgressData>();
        playerData = Load<PlayerData>();

        // 初回起動時にプレイヤーデータの新規作成
        if (string.IsNullOrEmpty(playerData.id)) playerData.Create();
    }
}