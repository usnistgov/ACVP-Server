using NIST.CVP.Math;
using System.Collections.Generic;

namespace NIST.CVP.Crypto.Common.Hash.TupleHash
{
    public class AlgoArrayResponse
    {
        public List<BitString> Tuple { get; set; }
        public string Customization { get; set; }
        public BitString Digest { get; set; }
        public int DigestLength => Digest.BitLength;
    }
}
