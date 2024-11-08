﻿using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes
{
    /// <summary>
    /// The result of a symmetric block cipher operation
    /// </summary>
    public interface IModeBlockCipherResult : ICryptoResult
    {
        /// <summary>
        /// The result from an encrypt/decrypt operation
        /// </summary>
        BitString Result { get; }
    }
}
