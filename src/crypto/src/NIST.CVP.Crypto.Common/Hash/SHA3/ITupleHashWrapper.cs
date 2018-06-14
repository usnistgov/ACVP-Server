using NIST.CVP.Math;
using System.Collections.Generic;

namespace NIST.CVP.Crypto.Common.Hash.SHA3
{
    public interface ITupleHashWrapper
    {
        BitString HashMessage(List<BitString> tuples, int digestSize, int capacity, bool XOF, string customization = "");
    }
}
