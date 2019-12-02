using System.Collections.Generic;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric
{
    public class SymmetricCounterResult : SymmetricCipherResult
    {
        public List<BitString> IVs { get; }

        public SymmetricCounterResult(BitString result, List<BitString> ivs)
            : base (result)
        {
            IVs = ivs;
        }

        public SymmetricCounterResult(string errorMessage) : base(errorMessage) { }
    }
}
