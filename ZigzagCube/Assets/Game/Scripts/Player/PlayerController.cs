using UnityEngine;

public class PlayerController : ControllerBase
{
    [SerializeField]
    private ModelController model;
    public ModelController Model => model;
    [SerializeField]
    private Collider[] colliders;

    public PlayerState State { get; private set; }
    
    protected override InputData CreateInputData()
    {
        InputData data = new InputData();
        if(InputHelper.IsPointerDown() && !InputHelper.IsPointerOverUI())
        {
            data.isTouch = true;
        }
        return data;
    }
    public override void SetActive(bool value)
    {
        TriggerCollider(value);
        base.SetActive(value);
    }
    /// <summary>
    /// 当たり判定の有効・無効の切り替え    </summary>
    private void TriggerCollider(bool isEnable)
    {
        if (colliders == null) return;
        foreach (Collider collider in colliders) collider.enabled = isEnable;
    }
    /// <summary>
    /// プレイヤーの状態変更    </summary>
    public void ChangeState(PlayerState newState)
    {
        if (State == newState) return;

        State = newState;
        switch(newState)
        {
            case PlayerState.Idle: SetActive(false); break;
            case PlayerState.Alive: SetActive(true); break;
            case PlayerState.Dying:
                {
                    Debug.Log("死亡処理の開始");
                    SetActive(false);
                    model.Hide();
                }
                break;
            case PlayerState.Death:
                {
                    Debug.Log("死亡処理の完了");
                    GameplayManager.Instance.GameOver();
                    ChangeState(PlayerState.Idle);
                }
                break;
            case PlayerState.Revive:
                {
                    Debug.Log("復活");
                    model.Show();
                    GetComponent<PlayerRevive>().Revive();
                }
                break;
        }
    }
}
