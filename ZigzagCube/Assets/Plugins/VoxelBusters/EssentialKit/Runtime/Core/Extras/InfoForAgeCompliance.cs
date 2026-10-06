using System;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// The <see cref="InfoForAgeCompliance"/> class provides information related to the age range of the user and other which are helpful to the age compliance process
    /// </summary>
    public class InfoForAgeCompliance
    {
        /// <summary>
        /// The age range of the user
        /// </summary>
        public AgeRange UserAgeRange { get; private set; }

        /// <summary>
        /// The age sharing status of the user
        /// </summary>
        public AgeSharingStatus AgeSharingStatus { get; private set; }

        /// <summary>
        /// The age range declaration method. See <see cref="AgeRangeDeclarationMethod"/> for possible values
        /// </summary>
        public AgeRangeDeclarationMethod UserAgeRangeDeclarationMethod { get; private set; }

        [Obsolete("Use the constructor with AgeSharingStatus instead", true)]
        public InfoForAgeCompliance(AgeRange userAgeRange, AgeRangeDeclarationMethod userAgeRangeDeclarationMethod)
        {
            UserAgeRange = userAgeRange;
            UserAgeRangeDeclarationMethod = userAgeRangeDeclarationMethod;
        }

        public InfoForAgeCompliance(AgeRange userAgeRange, AgeSharingStatus ageSharingStatus, AgeRangeDeclarationMethod userAgeRangeDeclarationMethod)
        {
            UserAgeRange = userAgeRange;
            AgeSharingStatus = ageSharingStatus;
            UserAgeRangeDeclarationMethod = userAgeRangeDeclarationMethod;
        }
        public override string ToString()
        {
            return string.Format("[UserAgeRange={0}, AgeSharingStatus={1}, UserAgeRangeDeclarationMethod={2}]", UserAgeRange, AgeSharingStatus, UserAgeRangeDeclarationMethod);
        }
    }
}