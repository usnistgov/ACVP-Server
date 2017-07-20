using NIST.CVP.Crypto.AES_ECB;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.DRBG
{
    public class DrbgCounterAes : DrbgCounterBase
    {
        protected IAES_ECB AesEcb;

        public DrbgCounterAes(IEntropyProvider entropyProvider, IAES_ECB aesEcb, DrbgParameters drbgParameters, int keyLength)
            : base(entropyProvider, drbgParameters, keyLength)
        {
            AesEcb = aesEcb;
            OutputLength = 128;
        }

        protected override BitString BlockEncrypt(BitString k, BitString x)
        {
            return AesEcb.BlockEncrypt(k, x).CipherText;
        }
    }
}