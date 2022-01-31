using System.Collections.Generic;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash
{
    public class MctResult<T>
    {
        public List<T> Response { get; set; }
        public string ErrorMessage { get; set; }
        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public MctResult(List<T> responses)
        {
            Response = responses;
        }

        public MctResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
