using System.Collections.Generic;

namespace NIST.CVP.Crypto.Common.Hash
{
    public class MCTResult<TAlgoArrayResponse>
        where TAlgoArrayResponse : AlgoArrayResponse
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
