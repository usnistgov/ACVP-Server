using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2
{
    public class ManyEncryptionResult : ICryptoResult
    {
        public List<AlgoArrayResponseSignature> AlgoArrayResponses { get; }

        public ManyEncryptionResult(List<AlgoArrayResponseSignature> list)
        {
            AlgoArrayResponses = list;
        }
    }
}
