using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2
{
    public class DecryptionResult : ICryptoResult
    {
        public BigInteger PlainText { get; }
        public string ErrorMessage { get; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public DecryptionResult(BigInteger pt)
        {
            PlainText = pt;
        }

        public DecryptionResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
