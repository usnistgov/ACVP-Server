using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF
{
    public class KdfResult : ICryptoResult
    {
        public BitString DerivedKey { get; }
        public string ErrorMessage { get; }
        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public KdfResult(BitString derivedKey)
        {
            DerivedKey = derivedKey;
        }

        public KdfResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}

