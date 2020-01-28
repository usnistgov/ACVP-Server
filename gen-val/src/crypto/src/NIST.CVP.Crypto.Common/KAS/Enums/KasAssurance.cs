using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.KAS.Enums
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
        [EnumMember(Value = "dpGen")]
        DpGen = 1 << 1,
        /// <summary>
        /// Domain Parameter Validation
        /// </summary>
        [EnumMember(Value = "dpVal")]
        DpVal = 1 << 2,
        /// <summary>
        /// Key Pair Generation
        /// </summary>
        [EnumMember(Value = "keyPairGen")]
        KeyPairGen = 1 << 3,
        /// <summary>
        /// Full Validation (FFC)
        /// </summary>
        [EnumMember(Value = "fullVal")]
        FullVal = 1 << 4,
        /// <summary>
        /// Partial Validation (ECC)
        /// </summary>
        [EnumMember(Value = "partialVal")]
        PartialVal = 1 << 5,
        /// <summary>
        /// Key Regeneration
        /// </summary>
        [EnumMember(Value = "keyRegen")]
        KeyRegen = 1 << 6
    }
}