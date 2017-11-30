using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.Common;

namespace NIST.CVP.Crypto.AES_CTR
{
    public class CtrResult : ICryptoResult
    {
        public List<AlgoArrayResponse> Response { get; }
        public string ErrorMessage { get; }

        public CtrResult(List<AlgoArrayResponse> result)
        {
            Response = result;
        }

        public CtrResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}
