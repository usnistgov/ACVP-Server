using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Math;
using System.Numerics;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class DsaDomainParametersResult : IResult
    {
        public BitString P { get; set; }
        public BitString Q { get; set; }
        public DomainSeed Seed { get; set; }
        public Counter Counter { get; set; }
        public BitString G { get; set; }
        public BitString H { get; set; }
        public BitString Index { get; set; }
    }
}
