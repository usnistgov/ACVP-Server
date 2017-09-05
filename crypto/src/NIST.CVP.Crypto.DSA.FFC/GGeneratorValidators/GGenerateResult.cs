using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NIST.CVP.Crypto.DSA.FFC.GGeneratorValidators
{
    public class GGenerateResult
    {
        public BigInteger G { get; }
        public string ErrorMessage { get; }

        public GGenerateResult(BigInteger g)
        {
            G = g;
        }

        public GGenerateResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}
