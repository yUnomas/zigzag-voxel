using UnityEngine;

public class ModelBase : MonoBehaviour
{
    [SerializeField, Tooltip("モデルのメインとなるマテリアル")]
    private Material mainMaterial;
    public Material MainMaterial => mainMaterial;

    public void Show() { gameObject.SetActive(true); }
    public void Hide() { gameObject.SetActive(false); }
}
