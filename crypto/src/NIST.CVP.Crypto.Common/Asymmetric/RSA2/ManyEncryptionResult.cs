using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2
{
    public class ManyEncryptionResult : ICryptoResult
    {
        public List<AlgoArrayResponse> AlgoArrayResponses { get; }

        public ManyEncryptionResult(List<AlgoArrayResponse> list)
        {
            AlgoArrayResponses = list;
        }
    }
}
