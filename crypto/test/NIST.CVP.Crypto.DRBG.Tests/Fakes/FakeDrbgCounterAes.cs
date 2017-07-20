using NIST.CVP.Crypto.AES_ECB;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.DRBG.Tests.Fakes
{
    public class FakeDrbgCounterAes : DrbgCounterAes
    {
        public FakeDrbgCounterAes(IEntropyProvider entropyProvider, IAES_ECB aesEcb, DrbgParameters drbgParameters, int keyLength) : base(entropyProvider, aesEcb, drbgParameters, keyLength)
        {
            AesEcb = aesEcb;
        }

        public void PublicBlockEncrypt(BitString k, BitString x)
        {
            BlockEncrypt(k, x);
        }
    }
}
