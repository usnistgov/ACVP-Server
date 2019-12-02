using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.DRBG.Tests.Fakes
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
