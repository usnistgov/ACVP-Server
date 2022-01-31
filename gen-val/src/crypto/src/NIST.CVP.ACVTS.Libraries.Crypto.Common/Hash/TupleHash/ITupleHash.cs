using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.TupleHash
{
    public interface ITupleHash
    {
        HashResult HashMessage(HashFunction hashFunction, IEnumerable<BitString> tuple, string customization);
        HashResult HashMessage(HashFunction hashFunction, IEnumerable<BitString> tuple, BitString customizationHex);
    }
}
