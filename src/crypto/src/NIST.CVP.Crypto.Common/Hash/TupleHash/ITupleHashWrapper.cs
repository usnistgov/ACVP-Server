using NIST.CVP.Math;
using System.Collections.Generic;

namespace NIST.CVP.Crypto.Common.Hash.TupleHash
{
    public interface ITupleHashWrapper
    {
        BitString HashMessage(IEnumerable<BitString> tuple, int digestSize, int capacity, bool XOF, string customization = "");
    }
}
