using NIST.CVP.Common;

namespace NIST.CVP.Generation.TDES_CFB.IntegrationTests
{
    public class GenValTestCfb1 : GenValTestsCfbBase
    {
        public override string Algorithm { get; } = "ACVP-TDES-CFB1";
        public override string Mode { get; } = string.Empty;

        public override AlgoMode AlgoMode => AlgoMode.TDES_CFB1_v1_0;
    }
}
