using System.Collections.Generic;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.CTR
{
    public class CounterEncryptionResult : ICryptoResult
    {
        public List<BitString> IVs { get; }
        public BitString CipherText { get; }
        public string ErrorMessage { get; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public CounterEncryptionResult(BitString cipherText, List<BitString> ivs)
        {
            CipherText = cipherText;
            IVs = ivs;
        }

        public CounterEncryptionResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
