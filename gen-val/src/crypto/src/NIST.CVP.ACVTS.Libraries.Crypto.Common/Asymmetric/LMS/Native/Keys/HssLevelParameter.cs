using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys
{
    /// <summary>
    /// One level's parameters used for the generation of an <see cref="IHssKeyPair"/>.
    /// </summary>
    public record HssLevelParameter
    {
        public HssLevelParameter(LmsMode lmsMode, LmOtsMode lmOtsMode)
        {
            LmsMode = lmsMode;
            LmOtsMode = lmOtsMode;
        }

        /// <summary>
        /// The <see cref="LmsMode"/> of the <see cref="ILmsKeyPair"/> to generate.
        /// </summary>
        public LmsMode LmsMode { get; }

        /// <summary>
        /// The <see cref="LmOtsMode"/> of the <see cref="ILmOtsKeyPair"/>s to generate for the level.
        /// </summary>
        public LmOtsMode LmOtsMode { get; }
    }
}
