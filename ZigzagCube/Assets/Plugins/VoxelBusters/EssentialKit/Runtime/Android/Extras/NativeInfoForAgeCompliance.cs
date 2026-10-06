#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;
using VoxelBusters.EssentialKit.Common.Android;

namespace VoxelBusters.EssentialKit.ExtrasCore.Android
{
    public class NativeInfoForAgeCompliance : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion
        #region Constructor

        // Default constructor
        public NativeInfoForAgeCompliance(AndroidJavaObject androidJavaObject) : base(Native.kClassName, androidJavaObject)
        {
        }
        public NativeInfoForAgeCompliance(NativeAndroidJavaObjectWrapper wrapper) : base(wrapper)
        {
        }
        public NativeInfoForAgeCompliance(NativeAgeRange userAgeRange, NativeAgeSharingStatus status, NativeAgeRangeDeclarationMethod userAgeRangeDeclarationMethod) : base(Native.kClassName ,(object)userAgeRange.NativeObject, (object)status, (object)userAgeRangeDeclarationMethod)
        {
        }

#if NATIVE_PLUGINS_DEBUG_ENABLED
        ~NativeInfoForAgeCompliance()
        {
            DebugLogger.Log("Disposing NativeInfoForAgeCompliance");
        }
#endif
        #endregion
        #region Static methods
        private static AndroidJavaClass GetClass()
        {
            if (m_nativeClass == null)
            {
                m_nativeClass = new AndroidJavaClass(Native.kClassName);
            }
            return m_nativeClass;
        }

        #endregion
        #region Public methods

        public NativeAgeSharingStatus GetAgeSharingStatus()
        {
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kGetAgeSharingStatus);
            NativeAgeSharingStatus data  = NativeAgeSharingStatusHelper.ReadFromValue(nativeObj);
            return data;
        }
        public NativeAgeRange GetUserAgeRange()
        {
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kGetUserAgeRange);
            NativeAgeRange data  = new  NativeAgeRange(nativeObj);
            return data;
        }
        public NativeAgeRangeDeclarationMethod GetUserAgeRangeDeclarationMethod()
        {
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kGetUserAgeRangeDeclarationMethod);
            NativeAgeRangeDeclarationMethod data  = NativeAgeRangeDeclarationMethodHelper.ReadFromValue(nativeObj);
            return data;
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.essentialkit.extras.InfoForAgeCompliance";

            internal class Method
            {
                internal const string kGetAgeSharingStatus = "getAgeSharingStatus";
                internal const string kGetUserAgeRange = "getUserAgeRange";
                internal const string kGetUserAgeRangeDeclarationMethod = "getUserAgeRangeDeclarationMethod";
            }

        }
    }
}
#endif