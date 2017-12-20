using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES
{
    public class DecryptionResultWithIv : DecryptionResult
    {
        public BitString[] IVs { get; set; }
        public BitString[] PlainTexts { get; set; }

        public DecryptionResultWithIv(BitString plainText, BitString[] _IVs) : base(plainText)
        {
            IVs = _IVs;
        }

        public DecryptionResultWithIv(BitString plainText, BitString[] _IVs, BitString[] _PlainTexts) : base(plainText)
        {
            IVs = _IVs;
            PlainTexts = _PlainTexts;
        }

        public DecryptionResultWithIv(string errorMessage) : base(errorMessage)
        {
        }
    }
}