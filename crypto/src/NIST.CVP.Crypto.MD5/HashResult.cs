using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.MD5
{
    public class HashResult
    {
        public BitString Digest { get; }
        public string ErrorMessage { get; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public HashResult(BitString digest)
        {
            Digest = digest;
        }

        public HashResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
