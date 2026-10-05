using UnityEngine;

public class PlayerController : ControllerBase
{
    [SerializeField]
    private ModelController model;
    public ModelController Model => model;
    [SerializeField]
    private Collider[] colliders;

    public PlayerState State => state;
    private PlayerState state;
    
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
        foreach (Collider collider in colliders) collider.enabled = isEnable;
    }
    public void ChangeState(PlayerState newState)
    {
        if (state == newState) return;

        state = newState;
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
