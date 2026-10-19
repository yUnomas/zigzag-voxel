using System;
using UnityEngine;

#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

public static class ATTManager
{
    public static void RequestAuthorization(Action onCompleted)
    {
#if UNITY_IOS && !UNITY_EDITOR
        var status =
            ATTrackingStatusBinding.GetAuthorizationTrackingStatus();

        if (status !=
            ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        {
            onCompleted?.Invoke();
            return;
        }

        ATTrackingStatusBinding.RequestAuthorizationTracking(
            result =>
            {
                await Awaitable.MainThreadAsync();

                Debug.Log($"ATT authorization status: {result}");
                onCompleted?.Invoke();
            });
#else
        onCompleted?.Invoke();
#endif
    }
}