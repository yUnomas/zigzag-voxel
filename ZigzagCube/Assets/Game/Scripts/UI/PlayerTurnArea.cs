using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerTurnArea : MonoBehaviour, IPointerClickHandler
{
    PlayerMovement player;

    private void Awake()
    {
        player = FindAnyObjectByType<PlayerMovement>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        player.Turn();
    }
}
