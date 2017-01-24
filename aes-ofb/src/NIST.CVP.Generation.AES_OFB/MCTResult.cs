using System.Collections.Generic;
using NIST.CVP.Generation.AES;

namespace NIST.CVP.Generation.AES_OFB
{
    public class MCTResult
    {
        public List<AlgoArrayResponse> Response { get; private set; }
        public string ErrorMessage { get; private set; }
        public MCTResult(List<AlgoArrayResponse> result)
        {
            Response = result;
        }

        public MCTResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success
        {
            get { return string.IsNullOrEmpty(ErrorMessage); }
        }
    }
}