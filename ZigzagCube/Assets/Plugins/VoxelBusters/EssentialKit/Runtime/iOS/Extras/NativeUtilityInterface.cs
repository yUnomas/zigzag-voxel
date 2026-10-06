#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.iOS;

namespace VoxelBusters.EssentialKit.ExtrasCore.iOS
{
    public class NativeUtilityInterface : NativeUtilityInterfaceBase
    {
        #region Constructors

        public NativeUtilityInterface()
            : base(isAvailable: true)
        { }

        #endregion

        #region Base methods

        public override void OpenAppStorePage(string applicationId)
        {
            string storeURL = string.Format("itms-apps://itunes.apple.com/app/id{0}", applicationId);
            Application.OpenURL(storeURL);
        }

        public override void OpenApplicationSettings()
        {
            IosNativePluginsUtility.OpenApplicationSettings();
        }

        public override void RequestInfoForAgeCompliance(RequestInfoForAgeComplianceOptions options, EventCallback<InfoForAgeCompliance> onComplete, AgeComplianceMockData mockData = null)
        {
            var tagPtr = MarshalUtility.GetIntPtr(onComplete);
            var nativeOptions = ExtrasUtility.From(options);
            var nativeMockData = ExtrasUtility.From(mockData);
            try

            {
                ExtrasBinding.NPExtrasRequestInfoForAgeCompliance(ref nativeOptions, ref nativeMockData, tagPtr, HandleRequestInfoForAgeComplianceCallback);
            }
            finally
            {
                nativeOptions.Dispose();
            }
        }

        #endregion

        #region Native Callbacks

        [MonoPInvokeCallback(typeof(RequestInfoForAgeComplianceNativeCallback))]
        private static void HandleRequestInfoForAgeComplianceCallback(NativeInfoForAgeComplianceData nativeData, NativeError nativeError, IntPtr tagPtr)
        {
            var     tagHandle       = GCHandle.FromIntPtr(tagPtr);
            try
            {
                var     infoForAgeComplianceObj = ExtrasUtility.From(nativeData);
                var errorObj = nativeError.Convert(ExtrasError.kDomain);
                EventCallback<InfoForAgeCompliance> callback = (EventCallback<InfoForAgeCompliance>)tagHandle.Target;
                CallbackDispatcher.InvokeOnMainThread(() =>
                {
                    callback.Invoke(infoForAgeComplianceObj, errorObj);
                });
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
            finally
            {
                // release handle
                tagHandle.Free();
            }
        }

        #endregion

    }
}
#endif