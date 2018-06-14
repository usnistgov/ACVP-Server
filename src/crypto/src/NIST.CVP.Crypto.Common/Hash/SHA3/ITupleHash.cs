using NIST.CVP.Math;
using System.Collections.Generic;

namespace NIST.CVP.Crypto.Common.Hash.SHA3
{
    public interface ITupleHash
    {
        HashResult HashMessage(HashFunction hashFunction, List<BitString> tuples, string customization = "");
    }
}
