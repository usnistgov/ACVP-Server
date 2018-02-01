using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.RSA2.Keys
{
    public class KeyResult
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
