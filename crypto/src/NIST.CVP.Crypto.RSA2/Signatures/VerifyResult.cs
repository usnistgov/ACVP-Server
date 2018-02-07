using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common;

namespace NIST.CVP.Crypto.RSA2.Signatures
{
    public class VerifyResult : ICryptoResult
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
