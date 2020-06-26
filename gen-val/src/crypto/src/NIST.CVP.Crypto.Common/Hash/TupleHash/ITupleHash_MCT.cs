using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using System.Collections.Generic;

namespace NIST.CVP.Crypto.Common.Hash.TupleHash
{
    public interface ITupleHash_MCT
    {
        MCTResultTuple<AlgoArrayResponse> MCTHash(HashFunction function, IEnumerable<BitString> tuple, MathDomain domain, bool hexCustomization, bool isSample);
    }
}
