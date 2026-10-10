using System.Collections.Generic;
using UnityEngine;

public class ModelController: MonoBehaviour
{
    [SerializeField]
    private ModelData currentModel;
    public ModelData Current => currentModel;
    [SerializeField]
    private List<ModelData> models = new List<ModelData>();

    /// <summary>
    /// 現在選択中のモデルのインデックス番号    </summary>
    private int currentModelIndex = 0;
    /// <summary>
    /// 現在のプレイヤーの移動方向    </summary>
    private int currentPlayerDirection = 1;

    /// <summary>
    /// モデル表示    </summary>
    public void Show() { currentModel.gameObject.SetActive(true); }
    /// <summary>
    /// モデル非表示    </summary>
    public void Hide() { currentModel.gameObject.SetActive(false); }

    public void Start()
    {
        // セーブデータからスキン反映
        SkinType skin = SaveDataManager.Instance.PlayerData.skin;
        for(int i = 0; i < models.Count; i++)
        {
            if (models[i].skinType == skin)
            {
                SwitchByIndex(i);
            }
        }
    }
    /// <summary>
    /// 指定番号のモデルに切り替え    </summary>
    private void SwitchByIndex(int index)
    {
        Hide();
        currentModel = models[index];
        Show();
        Turn(currentPlayerDirection);

        currentModelIndex = index;
    }
    /// <summary>
    /// 移動方向に対して適切な向きに調整    </summary>
    public void Turn(int direction)
    {
        currentPlayerDirection = direction;
        if (!currentModel.faceMovementDirection) return;

        currentModel.transform.rotation = Quaternion.Euler(0, 180f + -45f * direction, 0);
    }
    /// <summary>
    /// モデル切り替え    </summary>
    public void SwitchNext()
    {
        // 次のインデックス番号へ
        int nextModelIndex = (currentModelIndex + 1) % models.Count;
        // モデル切り替え
        SwitchByIndex(nextModelIndex);
        // 今回選択したモデルをセーブデータに保存
        PlayerData playerData = SaveDataManager.Instance.PlayerData;
        playerData.skin = currentModel.skinType;
        SaveDataManager.Instance.Save<PlayerData>(playerData);
    }
}