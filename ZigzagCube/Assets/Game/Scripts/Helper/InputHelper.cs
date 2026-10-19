using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class InputHelper
{
    /// <summary>
    /// ポインターの押下状態か判定   </summary>
    public static bool IsPointerDown()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return Input.GetMouseButtonDown(0);
#else
        return Input.touchCount > 0 &&
               Input.GetTouch(0).phase == TouchPhase.Began;
#endif
    }

    /// <summary>
    /// ポインターがUI上か判定    </summary>
    public static bool IsPointerOverUI()
    {
        if (EventSystem.current == null)
            return false;

        // ポインター位置の取得
        Vector2 position;
#if UNITY_EDITOR || UNITY_STANDALONE
        position = Input.mousePosition;
#else
        if (Input.touchCount == 0)
            return false;

        position = Input.GetTouch(0).position;
#endif
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = position
        };
        
        // レイの結果にUIが含まれているか判定
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach (RaycastResult result in results)
        {
            if (result.module is GraphicRaycaster)
                return true;
        }

        return false;
    }
}