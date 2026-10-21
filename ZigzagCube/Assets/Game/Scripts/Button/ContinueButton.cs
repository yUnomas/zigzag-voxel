using UnityEngine;

public class ContinueButton : UIButtonController
{
    private void Start()
    {
        // リワード広告の表示不可の場合はボタンを非表示
        if(AdsManager.Instance.CanShowAd(AdType.Rewarded))
        {
            gameObject.SetActive(false);
        }
        // ボタンを表示
        gameObject.SetActive(true);
    }
}
