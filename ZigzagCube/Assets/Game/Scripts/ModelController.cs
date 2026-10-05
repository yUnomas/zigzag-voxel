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
    public void Show() { gameObject.SetActive(true); }
    /// <summary>
    /// モデル非表示    </summary>
    public void Hide() { gameObject.SetActive(false); }

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
        // 前回選択していたモデルを非表示
        currentModel.gameObject.SetActive(false);
        // 今回選択するモデルを設定・表示
        currentModelIndex = (currentModelIndex + 1) % models.Count;
        currentModel = models[currentModelIndex];
        currentModel.gameObject.SetActive(true);
        Turn(currentPlayerDirection);
    }
}