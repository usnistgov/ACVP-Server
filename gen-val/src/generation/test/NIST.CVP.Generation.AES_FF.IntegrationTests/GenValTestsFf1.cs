using NIST.CVP.Common;

namespace NIST.CVP.Generation.AES_FF.IntegrationTests
{
    public class GenValTestsFf1 : GenValTestsBase
    {
        public override AlgoMode AlgoMode => AlgoMode.AES_FF1_v1_0;
        public override string Algorithm => "ACVP-AES-FF1";
    }
}