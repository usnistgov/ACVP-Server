using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_CFBP
{
    public class DecryptionResultWithIv : DecryptionResult
    {
        public BitString[] IVs { get; set; }
        public DecryptionResultWithIv(BitString plainText, BitString[] _IVs) : base(plainText)
        {
            IVs = _IVs;
        }

        public DecryptionResultWithIv(string errorMessage) : base(errorMessage)
        {
        }
    }
}