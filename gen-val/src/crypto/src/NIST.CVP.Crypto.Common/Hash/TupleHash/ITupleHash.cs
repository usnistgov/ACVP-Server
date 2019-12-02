using NIST.CVP.Math;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Hash.SHA3;

namespace NIST.CVP.Crypto.Common.Hash.TupleHash
{
    public interface ITupleHash
    {
        HashResult HashMessage(HashFunction hashFunction, IEnumerable<BitString> tuple, string customization);
        HashResult HashMessage(HashFunction hashFunction, IEnumerable<BitString> tuple, BitString customizationHex);
    }
}
