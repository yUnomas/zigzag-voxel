using System;
using System.Collections.Generic;
using GoogleMobileAds.Ump.Api;
using UnityEngine;

public static class UMPManager
{
    private static ConsentRequestParameters CreateConsentParameters()
    {
        ConsentRequestParameters parameters = new ConsentRequestParameters();

/*#if UNITY_IOS && !UNITY_EDITOR && DEVELOPMENT_BUILD
    parameters.ConsentDebugSettings = new ConsentDebugSettings
    {
        DebugGeography = DebugGeography.EEA,
        TestDeviceHashedIds = new List<string>
        {
            "YOUR_TEST_DEVICE_HASHED_ID"
        }
    };
#endif*/
        return parameters;
    }

    public static void RequestConsent(Action<bool> onCompleted)
    {
        // Create a ConsentRequestParameters object.
        ConsentRequestParameters requestParameters = CreateConsentParameters();

        // Request an update of the user's consent information.
        ConsentInformation.Update(requestParameters, async consentError =>
        {
            await Awaitable.MainThreadAsync();

            if (consentError != null)
            {
                Debug.LogError("Error updating consent information: " + consentError.Message);
                onCompleted?.Invoke(ConsentInformation.CanRequestAds());
                return;
            }

            ConsentForm.LoadAndShowConsentFormIfRequired((FormError formError) =>
            {
                if (formError != null)
                {
                    // Consent gathering failed.
                    Debug.LogError("Error loading/showing consent form: " + formError.Message);
                }
                onCompleted?.Invoke(ConsentInformation.CanRequestAds());
            });
        });
    }
    public static bool IsPrivacyOptionsRequired()
    {
        return ConsentInformation.PrivacyOptionsRequirementStatus
            == PrivacyOptionsRequirementStatus.Required;
    }
    public static void ShowPrivacyOptions()
    {
        ConsentForm.ShowPrivacyOptionsForm(error =>
        {
            if (error != null)
            {
                Debug.LogWarning("UMP Privacy Options Error: " + error.Message);
            }
        });
    }
}