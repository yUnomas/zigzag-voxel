#if UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.ExtrasCore.Android
{
    public class UtilityInterface : NativeUtilityInterfaceBase
    {
        #region Fields

        private NativeApplicationUtility m_applicationUtility;

        #endregion

        #region Constructors

        public UtilityInterface()
            : base(isAvailable: true)
        {
            m_applicationUtility = new NativeApplicationUtility(NativeUnityPluginUtility.GetContext());
        }

        #endregion

        #region Base methods        

        public override void OpenAppStorePage(string applicationId)
        {
            m_applicationUtility.OpenGooglePlayStoreLink(applicationId);
        }

        public override void OpenApplicationSettings()
        {
            m_applicationUtility.OpenApplicationSettings();
        }

        public override void RequestInfoForAgeCompliance(RequestInfoForAgeComplianceOptions options, EventCallback<InfoForAgeCompliance> onComplete, AgeComplianceMockData mockData = null)
        {
            var nativeOptions = GetNativeOptions(options);
            var nativeMockData = GetNativeMockData(mockData);
            m_applicationUtility.RequestInfoForAgeCompliance(nativeOptions, new NativeRequestInfoForAgeComplianceListener()
            {
                onSuccessCallback = (nativeInfoForAgeCompliance) =>
                {
                    AgeRange ageRange = new AgeRange(nativeInfoForAgeCompliance.GetUserAgeRange().GetLowerBound(), nativeInfoForAgeCompliance.GetUserAgeRange().GetUpperBound());
                    AgeSharingStatus ageSharingStatus = (AgeSharingStatus) nativeInfoForAgeCompliance.GetAgeSharingStatus();
                    AgeRangeDeclarationMethod ageRangeDeclarationMethod = (AgeRangeDeclarationMethod) nativeInfoForAgeCompliance.GetUserAgeRangeDeclarationMethod();
                     CallbackDispatcher.InvokeOnMainThread(() => onComplete(new InfoForAgeCompliance(ageRange, ageSharingStatus, ageRangeDeclarationMethod), null));  
                },
                onErrorCallback = (errorInfo) =>
                {
                    CallbackDispatcher.InvokeOnMainThread(() => onComplete(null, errorInfo.Convert(ExtrasError.kDomain)));
                }
            }, nativeMockData);
        }

        #endregion

        #region Private methods

        private NativeRequestInfoForAgeComplianceOptions GetNativeOptions(RequestInfoForAgeComplianceOptions options)
        {
            var nativeOptions = new NativeRequestInfoForAgeComplianceOptions();

            foreach (var each in options.AvailableContentAgeGates)
            {
                nativeOptions.AddContentAgeGateRange(each.LowerBound, each.UpperBound);
            }

            return nativeOptions;
        }   
        
        private NativeAgeComplianceMockData GetNativeMockData(AgeComplianceMockData mockData)
        {
            if (mockData == null)
            {
                return null;
            }

            var nativeMockData = new NativeAgeComplianceMockData(); 
            nativeMockData.SetAgeRange(mockData.AgeRange.LowerBound, mockData.AgeRange.UpperBound);
            nativeMockData.SetAgeRangeDeclarationMethod((int)mockData.AgeRangeDeclarationMethod);
            return nativeMockData;
        }

        #endregion
    }
}
#endif