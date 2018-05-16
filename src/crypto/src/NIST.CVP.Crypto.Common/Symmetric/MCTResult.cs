using System.Collections.Generic;

namespace NIST.CVP.Crypto.Common.Symmetric
{
    public class MCTResult<TAlgoArrayResponse> : IMCTResult<TAlgoArrayResponse> 
        where TAlgoArrayResponse : IAlgoArrayResponse
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