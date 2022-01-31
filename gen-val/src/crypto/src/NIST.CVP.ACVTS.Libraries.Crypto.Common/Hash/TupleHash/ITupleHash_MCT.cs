using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.TupleHash
{
    public interface ITupleHash_MCT
    {
        MCTResultTuple<AlgoArrayResponse> MCTHash(HashFunction function, IEnumerable<BitString> tuple, MathDomain domain, bool hexCustomization, bool isSample);
    }
}
