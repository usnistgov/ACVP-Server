using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NIST.CVP.Crypto.RSA2.Signatures
{
    public class SignatureResult
    {
        public BigInteger Signature { get; }
        public string ErrorMessage { get; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public SignatureResult(BigInteger signature)
        {
            Signature = signature;
        }

        public SignatureResult(string error)
        {
            ErrorMessage = error;
        }
    }
}
