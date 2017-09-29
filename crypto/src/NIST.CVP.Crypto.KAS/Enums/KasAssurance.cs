using System;

namespace NIST.CVP.Crypto.KAS.Enums
{
    /// <summary>
    /// Assurances that should be validated throughout the KAS flow
    /// </summary>
    [Flags]
    public enum KasAssurance
    {
        /// <summary>
        /// No Assurances associated with the KAS implementation
        /// </summary>
        None = 0,
        /// <summary>
        /// Domain Parameter Generation
        /// </summary>
        DpGen = 1 << 1,
        /// <summary>
        /// Domain Parameter Validation
        /// </summary>
        DpVal = 1 << 2,
        /// <summary>
        /// Key Pair Generation
        /// </summary>
        KeyPairGen = 1 << 3,
        /// <summary>
        /// Full Validation
        /// </summary>
        FullVal = 1 << 4,
        /// <summary>
        /// Key Regeneration
        /// </summary>
        KeyRegen = 1 << 5
    }
}