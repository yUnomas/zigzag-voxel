using UnityEngine;
using VoxelBusters.CoreLibrary;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField, Tooltip("最初の難易度上昇に必要な距離")]
    private float initialStepDistance = 50f;
    [SerializeField, Tooltip("難易度の上昇に必要な移動距離。以降は倍増")]
    private float distanceStepIncrease = 20f;
    [SerializeField, Tooltip("必要距離の増加がストップする難易度レベル")]
    private int maxStepLevel = 5;
    [SerializeField, Tooltip("難易度の上昇がストップする難易度レベル")]
    private int maxLevel = 10;
    [SerializeField, Tooltip("エフェクトのプレイヤー座標からのオフセット値")]
    private Vector3 effectOffset = new Vector3(-5, 6, 3);
    [Header("=====")]
    [SerializeField] private PlayerMovement player;
    [SerializeField] private DifficultyParameterTable table;
    [SerializeField] private EffectController leavesFX;
    [SerializeField] private EffectController windLineFX;

    public static DifficultyManager Instance => instance;
    private static DifficultyManager instance;

    /// <summary>
    /// 難易度パラメーター    </summary>
    public DifficultyParameterEntry Parameter => table.GetEntry(Level);
    /// <summary>
    /// 難易度レベル    </summary>
    public int Level { get; private set; }
    /// <summary>
    /// 現在の難易度上昇に必要な距離    </summary>
    private float currentStepDistance;
    /// <summary>
    /// 最後にの難易度上昇した際の距離    </summary>
    private float lastIncreasedDistance;

    private void Awake()
    {
        instance = this;
        currentStepDistance = initialStepDistance;
    }
    private void Update()
    {
        if (Level >= maxLevel) return;

        // 一定距離の移動で難易度を上昇
        if (player.transform.position.z - lastIncreasedDistance >= currentStepDistance)
        {
            IncreaseDifficulty();
            if (Level > 1) PlayEffect();
        }
    }
    private void LateUpdate()
    {
        // プレイヤーの位置 ＋ オフセット座標へ移動
        leavesFX.transform.position = player.transform.position + effectOffset;
        windLineFX.transform.position = player.transform.position + effectOffset;
    }

    private void IncreaseDifficulty()
    {
        Level++;
        Debug.Log($"難易度が{Level}に上昇しました");

        // プレイヤーの速度上昇
        player.SetSpeed(Parameter.playerMoveSpeed);

        // 次回必要な距離を設定
        float maxStepDistance = initialStepDistance + (distanceStepIncrease * maxStepLevel);
        currentStepDistance = Mathf.Min(currentStepDistance + distanceStepIncrease, maxStepDistance);
        // 今回の難易度上昇時の距離を保持
        lastIncreasedDistance = player.transform.position.z;
    }
    // エフェクト・SE再生
    private void PlayEffect()
    {
        leavesFX.Play();
        windLineFX.Play();
        AudioManager.Instance.PlaySE("Wind");
    }
}