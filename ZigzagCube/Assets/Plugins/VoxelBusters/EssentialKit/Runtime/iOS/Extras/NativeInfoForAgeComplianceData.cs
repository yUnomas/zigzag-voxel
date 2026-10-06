#if UNITY_IOS || UNITY_TVOS
using System;
using System.Runtime.InteropServices;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.ExtrasCore.iOS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NativeInfoForAgeComplianceData
    {
        #region Properties

        public AgeRange UserAgeRange { get; set; }

        public AgeSharingStatus AgeSharingStatus { get; set; }

        public AgeRangeDeclarationMethod UserAgeRangeDeclarationMethod { get; set; }

        #endregion
    }
}
#endif