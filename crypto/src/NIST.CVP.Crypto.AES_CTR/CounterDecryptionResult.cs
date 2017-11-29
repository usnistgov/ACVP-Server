using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.AES_CTR
{
    public class CounterDecryptionResult
    {
        public List<BitString> IVs { get; }
        public BitString CipherText { get; }
        public string ErrorMessage { get; }

        public bool Success => !string.IsNullOrEmpty(ErrorMessage);

        public CounterDecryptionResult(BitString cipherText, List<BitString> ivs)
        {
            CipherText = cipherText;
            IVs = ivs;
        }

        public CounterDecryptionResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
