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
        /// Domain Parameter Generation
        /// </summary>
        DpGen,
        /// <summary>
        /// Domain Parameter Validation
        /// </summary>
        DpVal,
        /// <summary>
        /// Key Pair Generation
        /// </summary>
        KeyPairGen,
        /// <summary>
        /// Full Validation
        /// </summary>
        FullVal,
        /// <summary>
        /// Key Regeneration
        /// </summary>
        KeyRegen
    }
}