﻿using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.DRBG.Tests.Fakes
{
    public class FakeDrbgCounterAes : DrbgCounterAes
    {
        public FakeDrbgCounterAes(IEntropyProvider entropyProvider, IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory cipherFactory, DrbgParameters drbgParameters) 
            : base(entropyProvider, engineFactory, cipherFactory, drbgParameters) { }

        public void PublicBlockEncrypt(BitString k, BitString x)
        {
            BlockEncrypt(k, x);
        }
    }
}