using NIST.CVP.Crypto.Common.Asymmetric.RSA2.PrimeGenerators;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys
{
    public class KeyResult : ICryptoResult
    {
        public KeyPair Key { get; }
        public AuxiliaryResult AuxValues { get; }
        public string ErrorMessage { get; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public KeyResult(KeyPair key, AuxiliaryResult aux)
        {
            Key = key;
            AuxValues = aux;
        }

        public KeyResult(string error)
        {
            ErrorMessage = error;
        }
    }
}
