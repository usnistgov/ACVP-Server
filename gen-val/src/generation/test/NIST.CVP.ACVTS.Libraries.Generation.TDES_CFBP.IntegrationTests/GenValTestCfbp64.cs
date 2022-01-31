using NIST.CVP.ACVTS.Libraries.Common;

namespace NIST.CVP.ACVTS.Libraries.Generation.TDES_CFBP.IntegrationTests
{
    public class GenValTestCfbp64 : GenValTestsCfbpBase
    {
        public override string Algorithm { get; } = "ACVP-TDES-CFBP64";
        public override string Mode { get; } = null;

        public override AlgoMode AlgoMode => AlgoMode.TDES_CFBP64_v1_0;
    }
}
