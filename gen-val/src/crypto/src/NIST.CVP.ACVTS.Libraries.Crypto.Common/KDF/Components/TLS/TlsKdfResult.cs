using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS
{
    public class TlsKdfResult
    {
        public BitString DerivedKey { get; }
        public BitString MasterSecret { get; }
        public string ErrorMessage { get; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public TlsKdfResult(BitString masterSecret, BitString derivedKey)
        {
            MasterSecret = masterSecret;
            DerivedKey = derivedKey;
        }

        public TlsKdfResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
