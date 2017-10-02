using System;
using System.ComponentModel;

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
        [Description("dpGen")]
        DpGen = 1 << 1,
        /// <summary>
        /// Domain Parameter Validation
        /// </summary>
        [Description("dpVal")]
        DpVal = 1 << 2,
        /// <summary>
        /// Key Pair Generation
        /// </summary>
        [Description("keyPairGen")]
        KeyPairGen = 1 << 3,
        /// <summary>
        /// Full Validation
        /// </summary>
        [Description("fullVal")]
        FullVal = 1 << 4,
        /// <summary>
        /// Key Regeneration
        /// </summary>
        [Description("keyRegen")]
        KeyRegen = 1 << 5
    }
}