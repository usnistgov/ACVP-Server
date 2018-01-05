using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.ANSIX963
{
    public class KdfResult
    {
        public BitString DerivedKey { get; }
        public string ErrorMessage { get; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public KdfResult(BitString key)
        {
            DerivedKey = key;
        }

        public KdfResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
