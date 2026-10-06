#if UNITY_IOS || UNITY_TVOS
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.ExtrasCore.iOS
{
    internal static class ExtrasUtility
    {
        internal static NativeRequestInfoForAgeComplianceOptionsData From(RequestInfoForAgeComplianceOptions options)
        {
            NativeRequestInfoForAgeComplianceOptionsData data = new();
            GCHandle ageGatesArray = GCHandle.Alloc(options.AvailableContentAgeGates, GCHandleType.Pinned);
            NativeArray nativeArray = new();
            nativeArray.Length = options.AvailableContentAgeGates.Length;
            nativeArray.Pointer = ageGatesArray.AddrOfPinnedObject();

            data.AvailableContentAgeGates = nativeArray;
            return data;
        }

        internal static InfoForAgeCompliance From(NativeInfoForAgeComplianceData nativeData)
        {
            AgeRange range = new AgeRange(nativeData.UserAgeRange.LowerBound, nativeData.UserAgeRange.UpperBound);
            var info = new InfoForAgeCompliance(range, nativeData.AgeSharingStatus, nativeData.UserAgeRangeDeclarationMethod);
            return info;
        }

        internal static NativeAgeComplianceMockData From(AgeComplianceMockData mockData)
        {
            var nativeMockData = new NativeAgeComplianceMockData
            {
                AgeRange = mockData == null ? new AgeRange(-1, -1) : mockData.AgeRange,
                AgeSharingStatus = mockData == null ? AgeSharingStatus.Unknown : mockData.AgeSharingStatus,
                AgeRangeDeclarationMethod = mockData == null ? AgeRangeDeclarationMethod.Unknown : mockData.AgeRangeDeclarationMethod
            };

            return nativeMockData;
        }
    }
}
#endif