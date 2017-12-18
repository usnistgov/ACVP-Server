using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common;

namespace NIST.CVP.Crypto.KDF
{
    public class KdfResult : ICryptoResult
    {
        public BitString DerivedKey { get; }
        public string ErrorMessage { get; }
        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public KdfResult(BitString derivedKey)
        {
            DerivedKey = derivedKey;
        }

        public KdfResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}

