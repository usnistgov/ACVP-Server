using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KDF
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

