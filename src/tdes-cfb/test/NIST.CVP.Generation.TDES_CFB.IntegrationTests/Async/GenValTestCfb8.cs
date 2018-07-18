using NIST.CVP.Common;

namespace NIST.CVP.Generation.TDES_CFB.IntegrationTests.Async
{
    public class GenValTestCfb8 : GenValTestsCfbBase
    {
        public override string Algorithm { get; } = "TDES";
        public override string Mode { get; } = "CFB8";

        public override AlgoMode AlgoMode => AlgoMode.TDES_CFB8;
    }
}
