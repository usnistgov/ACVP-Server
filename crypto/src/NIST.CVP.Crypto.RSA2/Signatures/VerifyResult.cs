using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.RSA2.Signatures
{
    public class VerifyResult
    {
        public string ErrorMessage { get; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public VerifyResult() { }

        public VerifyResult(string error)
        {
            ErrorMessage = error;
        }
    }
}
