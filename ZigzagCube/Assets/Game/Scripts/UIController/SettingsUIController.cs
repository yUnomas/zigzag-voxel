using TMPro;
using UnityEngine;

public class SettingsUIController : UIControllerBase
{
    [SerializeField] private GameObject privacyOptionsButton;

    private PlayerNameField playerNameField;
    private SaveDataManager saveDataManager;

    public override void Show()
    {
        gameObject.SetActive(true);
        UpdatePrivacyOptionsButton();
        /* プレイヤー名を取得・フィールド内に表示
        saveDataManager = SaveDataManager.Instance;
        playerNameField.SetPlayerName(saveDataManager.PlayerData.name);
        */
    }
    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    private void UpdatePrivacyOptionsButton()
    {
        // UMPの同意状態によって表示状態を更新
        privacyOptionsButton.SetActive(UMPManager.Instance.IsPrivacyOptionsRequired());
    }
    public void OnClickPrivacyOptions()
    {
        UMPManager.Instance.ShowPrivacyOptions();
    }
    /// <summary>
    /// プレイヤー名の変更完了時のイベント    </summary>
    public void OnEndEditPlayerName(TMP_InputField inputField)
    {
        // 空欄の場合は元のプレイヤー名に戻す
        if (string.IsNullOrEmpty(inputField.text))
        {
            inputField.text = saveDataManager.PlayerData.name;
            return;
        }
        // 入力されたプレイヤー名に変更・保存
        saveDataManager.PlayerData.name = inputField.text;
        saveDataManager.Save(saveDataManager.PlayerData);
    }
}
