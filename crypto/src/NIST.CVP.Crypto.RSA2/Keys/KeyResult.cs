using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common;

namespace NIST.CVP.Crypto.RSA2.Keys
{
    public class KeyResult : ICryptoResult
    {
        public KeyPair Key { get; }
        public string ErrorMessage { get; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public KeyResult(KeyPair key)
        {
            Key = key;
        }

        public KeyResult(string error)
        {
            ErrorMessage = error;
        }
    }
}
