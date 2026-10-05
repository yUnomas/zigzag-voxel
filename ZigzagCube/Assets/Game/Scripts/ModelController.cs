using System.Collections.Generic;
using UnityEngine;

public class ModelController: MonoBehaviour
{
    [SerializeField]
    private ModelData currentModel;
    public ModelData Current => currentModel;
    [SerializeField]
    private List<ModelData> models = new List<ModelData>();

    private int currentModelIndex = 0;

    public void Show() { gameObject.SetActive(true); }
    public void Hide() { gameObject.SetActive(false); }

    public void Turn(int direction)
    {
        if (!currentModel.faceMovementDirection) return;

        currentModel.transform.rotation = Quaternion.Euler(0, 180f + -45f * direction, 0);
    }
    public void SwitchNext()
    {
        // 前回選択していたモデルを非表示
        currentModel.gameObject.SetActive(false);
        // 今回選択するモデルを設定・表示
        currentModelIndex = (currentModelIndex + 1) % models.Count;
        currentModel = models[currentModelIndex];
        currentModel.gameObject.SetActive(true);
    }
}