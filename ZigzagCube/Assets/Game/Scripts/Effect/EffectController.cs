using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField, Tooltip("終了後の削除切り替え")]
    private bool destroyOnFinished;
    [Header("=====")]
    [SerializeField] protected ParticleSystem effect;

    protected virtual void Awake()
    {
        // 参照が未設定の場合は取得
        if (effect == null) effect = GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// エフェクト削除 </summary>
    /// <param name="t">
    /// 削除までの期間 </param>
    protected virtual void DestroyEffect(float delay = 0f)
    {
        Destroy(gameObject, delay);
    }
    /// <summary>
    /// エフェクト再生    </summary>
    public virtual void Play()
    {
        effect.Play();
        if (destroyOnFinished)
        {
            float totalLifetime = effect.main.duration + effect.main.startLifetime.constantMax;
            DestroyEffect(totalLifetime);
        }
    }
    /// <summary>
    /// エフェクト再生    </summary>
    public virtual void Play(Vector3 pos, Quaternion rotation)
    {
        transform.position = pos;
        transform.rotation = rotation;
        Play();
    }
    /// <summary>
    /// エフェクトをクリア    </summary>
    public virtual void Clear()
    {
        effect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public virtual void SetMaterial(Material newMaterial)
    {
        var particleRenderer = effect.GetComponent<ParticleSystemRenderer>();
        if (particleRenderer != null)
        {
            particleRenderer.material = newMaterial;
        }
    }
}