using System.Collections.Generic;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA
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
