using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2
{
    public class EncryptionResult : ICryptoResult
    {
        public BigInteger CipherText { get; }
        public string ErrorMessage { get; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public EncryptionResult(BigInteger ct)
        {
            CipherText = ct;
        }

        public EncryptionResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
