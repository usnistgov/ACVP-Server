using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.CTR
{
    public class CounterDecryptionResult : ICryptoResult
    {
        public List<BitString> IVs { get; }
        public BitString PlainText { get; }
        public string ErrorMessage { get; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public CounterDecryptionResult(BitString plainText, List<BitString> ivs)
        {
            PlainText = plainText;
            IVs = ivs;
        }

        public CounterDecryptionResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
