using System.Collections.Generic;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA
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
