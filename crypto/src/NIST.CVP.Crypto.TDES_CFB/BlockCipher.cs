using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_CFB
{
    public abstract class BlockCipher : ICipher
    {
        public abstract BitString BlockEncrypt();
        public abstract BitString BlockDecrypt();
    }
}