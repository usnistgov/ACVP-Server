using NIST.CVP.Common;

namespace NIST.CVP.Generation.TDES_CFBP.IntegrationTests
{
    public class GenValTestCfbp8 : GenValTestsCfbpBase
    {
        public override string Algorithm { get; } = "ACVP-TDES-CFBP8";
        public override string Mode { get; } = string.Empty;

        public override AlgoMode AlgoMode => AlgoMode.TDES_CFBP8_v1_0;
    }
}
