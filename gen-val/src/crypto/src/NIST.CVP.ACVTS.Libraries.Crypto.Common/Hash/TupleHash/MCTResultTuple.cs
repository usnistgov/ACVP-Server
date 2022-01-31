using System.Collections.Generic;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.TupleHash
{
    public class MCTResultTuple<TAlgoArrayResponse>
        where TAlgoArrayResponse : AlgoArrayResponse
    {
        public List<TAlgoArrayResponse> Response { get; }
        public string ErrorMessage { get; }
        public MCTResultTuple(List<TAlgoArrayResponse> result)
        {
            Response = result;
        }

        public MCTResultTuple(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}
