using System;
using System.Collections.Generic;

namespace NIST.CVP.Crypto.Common.Symmetric.TDES
{
    [Obsolete("Being replaced by Symmetric.MCTResult")]
    public class MCTResult<TAlgoArrayResponse> : IMCTResult<TAlgoArrayResponse>
        where TAlgoArrayResponse : ICryptoResult
    {
        public List<TAlgoArrayResponse> Response { get;}
        public string ErrorMessage { get;}
        public MCTResult(List<TAlgoArrayResponse> result)
        {
            Response = result;
        }

        public MCTResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}