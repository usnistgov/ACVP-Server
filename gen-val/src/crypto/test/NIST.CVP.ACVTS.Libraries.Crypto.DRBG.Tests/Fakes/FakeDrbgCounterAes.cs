using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.DRBG.Tests.Fakes
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
