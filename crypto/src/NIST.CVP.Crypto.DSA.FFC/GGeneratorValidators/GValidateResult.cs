using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.DSA.FFC.GGeneratorValidators
{
    public class GValidateResult
    {
        public string ErrorMessage { get; }

        public GValidateResult() { }

        public GValidateResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}
