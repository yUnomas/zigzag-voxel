using UnityEngine;

public class ContinueButton : UIButtonController
{
    private void Start()
    {
        // ボタンを非表示
        gameObject.SetActive(false);
        // リワード広告の表示可の場合にボタンを表示
        if (AdsManager.Instance.CanShowAd(AdType.Rewarded))
        {
            gameObject.SetActive(true);
        }
    }
}
