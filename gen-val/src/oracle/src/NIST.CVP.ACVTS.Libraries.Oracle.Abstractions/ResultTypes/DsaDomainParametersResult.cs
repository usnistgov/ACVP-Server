using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
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
