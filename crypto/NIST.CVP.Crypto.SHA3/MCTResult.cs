using System.Collections.Generic;

namespace NIST.CVP.Crypto.SHA3
{
    public class MCTResult<TAlgoArrayResponse> where TAlgoArrayResponse : AlgoArrayResponse
    {
        public List<TAlgoArrayResponse> Response { get; private set; }
        public string ErrorMessage { get; private set; }

        public MCTResult(List<TAlgoArrayResponse> result)
        {
            Response = result;
        }

        public MCTResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success { get { return string.IsNullOrEmpty(ErrorMessage); } }
    }
}
