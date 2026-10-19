using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private List<ChunkController> chunks = new List<ChunkController>();
    [SerializeField] private GameObject editorGuideObjects;
    [Header("Systems")]
    [SerializeField] private PlayerController player;
    [SerializeField] private DifficultyManager difficultyManager;

    public static FieldManager Instance => instance;
    private static FieldManager instance;

    private void Awake()
    {
        instance = this;
        if(editorGuideObjects) Destroy(editorGuideObjects);
    }
    private void Start()
    {
        Generate();
    }
    private void LateUpdate()
    {
        CheckChunk();
    }

    /// <summary>
    /// フィールド生成    </summary>
    private void Generate()
    {
        // 各チャンクを生成
        foreach (var chunk in chunks)
        {
            chunk.Regenerate(false, chunks.Count, difficultyManager);
        }
    }
    /// <summary>
    /// チャンクを再生成するか確認   </summary>
    private void CheckChunk()
    {
        // プレイヤーから一定以上離れたら再生成
        foreach (ChunkController chunk in chunks)
        {
            if (player.transform.position.z - chunk.transform.position.z >= chunk.Length * 2)
            {
                chunk.Regenerate(true, chunks.Count, difficultyManager);
            }
        }
    }
    /// <summary>
    /// プレイヤーが復活するチャンクの準備    </summary>
    /// <returns>
    /// 復活地点   </returns>
    public Vector3Int PrepareRevivePoint()
    {
        // プレイヤー座標よりも手前にある最も近いチャンクを取得
        ChunkController reviveChunk = null;
        foreach(ChunkController chunk in chunks)
        {
            if(chunk.transform.position.z < player.transform.position.z &&
                (reviveChunk == null || reviveChunk.transform.position.z < chunk.transform.position.z))
            {
                reviveChunk = chunk;
            }
        }

        // 安全なチャンクに再生成
        reviveChunk?.Regenerate(false, chunks.Count, difficultyManager, ChunkType.Start);
        return reviveChunk.RevivePoint;
    }
}
