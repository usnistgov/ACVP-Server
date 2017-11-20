using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_CFBP
{
    public class EncryptionResultWithIv : EncryptionResult
    {
        public BitString[] IVs { get; set; }
        public EncryptionResultWithIv(BitString cipherText, BitString[] _IVs) : base(cipherText)
        {
            IVs = _IVs;
        }

        public EncryptionResultWithIv(string errorMessage) : base(errorMessage)
        {
        }
    }
}