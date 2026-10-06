using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.ExtrasCore.Simulator
{
    public sealed class UtilityInterface : NativeFeatureInterfaceBase, INativeUtilityInterface
    {
        public UtilityInterface() : base(true)
        {
        }

        public void OpenApplicationSettings()
        {
            Diagnostics.LogNotSupportedInEditor("OpenApplicationSettings");
        }

        public void OpenAppStorePage(string applicationId) //TODO: Abstract platform store url building to common library as it's used in rate my app as well.
        {
            var     activePlatform  = PlatformMappingServices.GetActivePlatform();
            switch (activePlatform)
            {
                case NativePlatform.Android:
                    Application.OpenURL("https://play.google.com/store/apps/details?id=" + applicationId); 
                    break;

                case NativePlatform.iOS:
                case NativePlatform.tvOS:
                    Application.OpenURL($"https://apps.apple.com/app/id{applicationId}?action=write-review");
                    break;
                default:
                    DebugLogger.LogWarning(EssentialKitDomain.Default, "Cannot open app store page. Unsupported platform: " + activePlatform.ToString());
                    Application.OpenURL("https://google.com/search?" + applicationId);
                    break;
            }
        }

        public void RequestInfoForAgeCompliance(RequestInfoForAgeComplianceOptions options, EventCallback<InfoForAgeCompliance> onComplete, AgeComplianceMockData mockData = null)
        {

            if (mockData == null)
            {
                onComplete(new InfoForAgeCompliance(new AgeRange(0, 100), AgeSharingStatus.Unknown, AgeRangeDeclarationMethod.Unknown), null);
            }
            else
            {
                InfoForAgeCompliance infoForAgeCompliance = new InfoForAgeCompliance(mockData.AgeRange, mockData.AgeSharingStatus, mockData.AgeRangeDeclarationMethod);
                onComplete(infoForAgeCompliance, null);
            }
        }
    }
}