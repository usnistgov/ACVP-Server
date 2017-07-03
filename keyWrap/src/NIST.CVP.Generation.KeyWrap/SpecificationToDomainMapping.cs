using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.KeyWrap.Enums;

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
        public static readonly List<(string algorithm, KeyWrapType keyWrapType)> Map =
            new List<(string algorithm, KeyWrapType keyWrapType)>()
            {
                ("AES-KW", KeyWrapType.AES_KW)
            };
    }
}
