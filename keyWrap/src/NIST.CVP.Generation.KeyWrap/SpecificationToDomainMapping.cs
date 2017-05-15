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
        /// algorithm, KeyWrapType
        /// TODO Update this with strongly typed tuple implementation from C# 7
        /// </summary>
        public static readonly List<Tuple<string, KeyWrapType>> Map =
            new List<Tuple<string, KeyWrapType>>()
            {
                new Tuple<string, KeyWrapType>("AES-KW", KeyWrapType.AES_KW)
            };
    }
}
