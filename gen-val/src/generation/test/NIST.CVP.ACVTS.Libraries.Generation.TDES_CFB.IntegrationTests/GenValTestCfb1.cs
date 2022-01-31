using NIST.CVP.ACVTS.Libraries.Common;

namespace NIST.CVP.ACVTS.Libraries.Generation.TDES_CFB.IntegrationTests
{
    public class GenValTestCfb1 : GenValTestsCfbBase
    {
        public override string Algorithm { get; } = "ACVP-TDES-CFB1";
        public override string Mode { get; } = null;

        public override AlgoMode AlgoMode => AlgoMode.TDES_CFB1_v1_0;
    }
}
