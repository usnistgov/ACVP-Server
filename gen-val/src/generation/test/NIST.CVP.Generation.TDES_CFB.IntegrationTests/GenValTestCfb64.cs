using NIST.CVP.Common;

namespace NIST.CVP.Generation.TDES_CFB.IntegrationTests
{
    public class GenValTestCfb64 : GenValTestsCfbBase
    {
        public override string Algorithm { get; } = "ACVP-TDES-CFB64";
        public override string Mode { get; } = string.Empty;

        public override AlgoMode AlgoMode => AlgoMode.TDES_CFB64_v1_0;
    }
}
