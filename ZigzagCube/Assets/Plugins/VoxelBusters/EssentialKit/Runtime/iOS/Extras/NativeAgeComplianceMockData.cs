#if UNITY_IOS || UNITY_TVOS
using System.Runtime.InteropServices;

namespace VoxelBusters.EssentialKit.ExtrasCore.iOS
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct NativeAgeComplianceMockData
    {
        #region Properties

        public AgeRange AgeRange { get;  internal set; }
        public AgeSharingStatus AgeSharingStatus { get;  internal set; }
        public AgeRangeDeclarationMethod AgeRangeDeclarationMethod { get;  internal set; }

        #endregion
    }

}
#endif