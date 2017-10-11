using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public class EccDomainParametersValidateResult : IDomainParametersValidateResult
    {
        public string ErrorMessage { get; }

        public EccDomainParametersValidateResult() { }

        public EccDomainParametersValidateResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}
