﻿using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.Common.DRBG
{
    /// <summary>
    /// Provides a means to retrieve a DRBG implementation
    /// </summary>
    public interface IDrbgFactory
    {
        IDrbg GetDrbgInstance(DrbgParameters drbgParameters, IEntropyProvider iEntropyProvider);
    }
}