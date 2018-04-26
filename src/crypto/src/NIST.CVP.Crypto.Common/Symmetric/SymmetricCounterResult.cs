using System.Collections.Generic;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric
{
    public class SymmetricCounterResult : ICryptoResult
    {
        public List<BitString> IVs { get; }
        public BitString Result { get; }
        public string ErrorMessage { get; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public SymmetricCounterResult(BitString result, List<BitString> ivs)
        {
            Result = result;
            IVs = ivs;
        }

        public SymmetricCounterResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
