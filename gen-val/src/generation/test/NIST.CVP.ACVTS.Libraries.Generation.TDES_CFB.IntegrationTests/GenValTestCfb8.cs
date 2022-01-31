using NIST.CVP.ACVTS.Libraries.Common;

namespace NIST.CVP.ACVTS.Libraries.Generation.TDES_CFB.IntegrationTests
{
    public class GenValTestCfb8 : GenValTestsCfbBase
    {
        public override string Algorithm { get; } = "ACVP-TDES-CFB8";
        public override string Mode { get; } = null;

        public override AlgoMode AlgoMode => AlgoMode.TDES_CFB8_v1_0;
    }
}
