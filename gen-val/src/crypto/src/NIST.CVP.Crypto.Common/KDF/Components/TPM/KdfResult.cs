using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KDF.Components.TPM
{
    public class KdfResult
    {
        public BitString SKey { get; }
        public string ErrorMessage { get; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public KdfResult(BitString skey)
        {
            SKey = skey;
        }

        public KdfResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
