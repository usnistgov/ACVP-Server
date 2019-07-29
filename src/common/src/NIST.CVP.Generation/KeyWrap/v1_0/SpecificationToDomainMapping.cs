using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric.KeyWrap.Enums;

namespace NIST.CVP.Generation.KeyWrap.v1_0
{
    /// <summary>
    /// Used to help map between the ACVP specification (and subsequent json files)
    /// To their strongly typed implementation on the gev/vals
    /// </summary>
    public class SpecificationToDomainMapping
    {
        /// <summary>
        /// Maps algorithms to a KeyWrapType
        /// </summary>
        public static readonly List<(string algorithm, KeyWrapType keyWrapType)> Map =
            new List<(string algorithm, KeyWrapType keyWrapType)>()
            {
                ("ACVP-AES-KW", KeyWrapType.AES_KW),
                ("ACVP-TDES-KW", KeyWrapType.TDES_KW),
                ("ACVP-AES-KWP", KeyWrapType.AES_KWP)
            };
    }
}
