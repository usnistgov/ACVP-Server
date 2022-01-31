using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CBC_CTS.v1_0
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
        public static readonly List<(AlgoMode algoMode, BlockCipherModesOfOperation modeOfOperation)> Map =
            new List<(AlgoMode algoMode, BlockCipherModesOfOperation modeOfOperation)>()
            {
                (AlgoMode.AES_CBC_CS1_v1_0, BlockCipherModesOfOperation.CbcCs1),
                (AlgoMode.AES_CBC_CS2_v1_0, BlockCipherModesOfOperation.CbcCs2),
                (AlgoMode.AES_CBC_CS3_v1_0, BlockCipherModesOfOperation.CbcCs3)
            };

        public static AlgoMode GetAlgoModeFromModeOfOperation(BlockCipherModesOfOperation mode)
        {
            if (!Map
                .TryFirst(
                    w => w.modeOfOperation.Equals(mode),
                    out var result))
            {
                throw new ArgumentException("Invalid Algorithm provided.");
            }

            return result.algoMode;
        }

        public static BlockCipherModesOfOperation GetModeOfOperationFromAlgoMode(AlgoMode mode)
        {
            if (!Map
                .TryFirst(
                    w => w.algoMode.Equals(mode),
                    out var result))
            {
                throw new ArgumentException("Invalid Algorithm provided.");
            }

            return result.modeOfOperation;
        }
    }
}
