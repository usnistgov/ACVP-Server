using NIST.CVP.Crypto.AES_ECB;
using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.DRBG.Tests.Fakes
{
    public class FakeDrbgCounterAes : DrbgCounterAes
    {
        public FakeDrbgCounterAes(IEntropyProvider entropyProvider, IAES_ECB aesEcb, DrbgParameters drbgParameters) : base(entropyProvider, aesEcb, drbgParameters)
        {
            AesEcb = aesEcb;
        }

        public void PublicBlockEncrypt(BitString k, BitString x)
        {
            BlockEncrypt(k, x);
        }
    }
}
