using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.TupleHash
{
    public interface ITupleHashWrapper
    {
        BitString HashMessage(IEnumerable<BitString> tuple, int digestLength, int capacity, bool XOF, string customization = "");
        BitString HashMessage(IEnumerable<BitString> tuple, int digestLength, int capacity, bool XOF, BitString customizationHex);
    }
}
