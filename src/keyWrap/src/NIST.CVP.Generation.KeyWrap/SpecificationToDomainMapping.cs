using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Symmetric.KeyWrap.Enums;

namespace NIST.CVP.Generation.KeyWrap
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
        public static readonly List<(string algorithm, string mode, KeyWrapType keyWrapType)> Map =
            new List<(string algorithm, string mode, KeyWrapType keyWrapType)>()
            {
                ("AES-KW", string.Empty, KeyWrapType.AES_KW),
                ("TDES-KW", string.Empty, KeyWrapType.TDES_KW),
                ("AES-KWP", string.Empty, KeyWrapType.AES_KWP)
            };
    }
}
