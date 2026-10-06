#if UNITY_ANDROID
using UnityEngine;
using System.Collections.Generic;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.ExtrasCore.Android
{
    public enum NativeAgeSharingStatus
    {
        Unknown = 0,
        Shared = 1,
        NotShared = 2,
        VerificationPending = 3,
        NotApplicable = 4
    }
    public class NativeAgeSharingStatusHelper
    {
        internal const string kClassName = "com.voxelbusters.essentialkit.extras.AgeSharingStatus";

        public static AndroidJavaObject CreateWithValue(NativeAgeSharingStatus value)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[NativeAgeSharingStatusHelper : NativeAgeSharingStatusHelper][Method(CreateWithValue) : NativeAgeSharingStatus]");
#endif
            AndroidJavaClass javaClass = new AndroidJavaClass(kClassName);
            AndroidJavaObject[] values = javaClass.CallStatic<AndroidJavaObject[]>("values");
            return values[(int)value];
        }

        public static NativeAgeSharingStatus ReadFromValue(AndroidJavaObject value)
        {
            return (NativeAgeSharingStatus)value.Call<int>("ordinal");
        }
    }
}
#endif