using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.DRBG.Tests.Fakes
{
    public class FakeDrbgCounterTdes : DrbgCounterTdes
    {
        public FakeDrbgCounterTdes(IEntropyProvider entropyProvider, IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory cipherFactory, DrbgParameters drbgParameters)
            : base(entropyProvider, engineFactory, cipherFactory, drbgParameters) { }

        public void PublicBlockEncrypt(BitString k, BitString x)
        {
            BlockEncrypt(k, x);
        }

        public BitString Convert168BitKeyTo192BitKey_public(BitString key)
        {
            return Convert168BitKeyTo192BitKey(key);
        }
    }
}
