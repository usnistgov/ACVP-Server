using NIST.CVP.ACVTS.Libraries.Common;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_FF.IntegrationTests
{
    public class GenValTestsFf3_1 : GenValTestsBase
    {
        public override AlgoMode AlgoMode => AlgoMode.AES_FF1_v1_0;
        public override string Algorithm => "ACVP-AES-FF3-1";
    }
}
