using NIST.CVP.Crypto.AES_ECB;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.DRBG
{
    public class DrbgCounterAes : DrbgCounterBase
    {
        private readonly IAES_ECB _aesEcb;

        public DrbgCounterAes(IEntropyProvider entropyProvider, IAES_ECB aesEcb, DrbgParameters drbgParameters, int keyLength)
            : base(entropyProvider, drbgParameters, keyLength)
        {
            _aesEcb = aesEcb;
            OutputLength = 128;
        }

        protected override BitString BlockEncrypt(BitString K, BitString X)
        {
            return _aesEcb.BlockEncrypt(K, X).CipherText;
        }
    }
}