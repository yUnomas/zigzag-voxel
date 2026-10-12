using System.Collections.Generic;
using UnityEngine;

public abstract class ControllerBase : MonoBehaviour
{
    [SerializeField, Tooltip("アクティブ状態")]
    private bool isActive = true;
    public bool IsActive => isActive;
    [SerializeField, Tooltip("モジュールリスト")]
    protected List<ModuleBase> modules = new List<ModuleBase>();

    protected virtual void Start()
    {
        // 初期化
        foreach (var module in modules)
        {
            module.SetController(this);
            module.Initialize();

            // 初期状態に応じてActivate/Deactivateを実行
            if (isActive) module.Activate();
            else module.Deactivate();
        }
    }
    protected virtual void Update()
    {
        if (!isActive) return;

        // 入力情報の取得
        InputData inputData = CreateInputData();
        // 各挙動の実行
        foreach (var module in modules)
        {
            module.Execute(inputData);
        }
    }
    protected virtual void FixedUpdate()
    {
        if (!isActive) return;

        // 各挙動の実行
        foreach (var module in modules)
        {
            module.FixedExecute();
        }
    }

    /// <summary>
    /// アクティブを設定    </summary>
    public virtual void SetActive(bool value)
    {
        if (isActive == value) return;

        isActive = value;
        foreach (var module in modules)
        {
            if (value) module.Activate();
            else module.Deactivate();
        }
    }
    /// <summary>
    /// 特定モデュールを取得    </summary>
    public T GetModule<T>() where T : ModuleBase
    {
        foreach(var module in modules)
        {
            // moduleをT型でキャストし、nullでなければ渡す
            if (module is T typedModule) return typedModule;
        }
        Debug.LogError(
            $"一致するモデュールがセットされていません。\nObject:{gameObject} Module:{typeof(T)}");
        return null;
    }

    /// <summary>
    /// 入力情報の作成    </summary>
    protected virtual InputData CreateInputData()
    {
        return new InputData();
    }
}