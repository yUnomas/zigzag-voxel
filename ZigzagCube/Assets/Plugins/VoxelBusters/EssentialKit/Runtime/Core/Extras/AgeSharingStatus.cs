namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// The <see cref="AgeSharingStatus"/> enum represents the age sharing status
    /// </summary>
    public enum AgeSharingStatus
    {
        /// <summary>
        /// The age sharing status is not known
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// User or guardian has shared the age information
        /// </summary>
        Shared = 1,

        /// <summary>
        /// User or guardian has not shared the age information
        /// </summary>
        NotShared = 2,

        /// <summary>
        /// The age sharing status is verification pending
        /// </summary>
        VerificationPending = 3,

        /// <summary>
        /// The age sharing status is not applicable
        /// </summary>
        NotApplicable = 4
    }
}