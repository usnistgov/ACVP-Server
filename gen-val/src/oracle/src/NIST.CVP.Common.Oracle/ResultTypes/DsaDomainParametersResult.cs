using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Math;
using System.Numerics;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class DsaDomainParametersResult : IResult
    {
        public BigInteger P { get; set; }
        public BigInteger Q { get; set; }
        public DomainSeed Seed { get; set; }
        public Counter Counter { get; set; }
        public BigInteger G { get; set; }
        public BigInteger H { get; set; }
        public BitString Index { get; set; }
    }
}
