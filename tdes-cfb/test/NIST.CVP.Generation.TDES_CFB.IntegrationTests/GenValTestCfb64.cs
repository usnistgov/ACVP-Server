using NIST.CVP.Common;
using NIST.CVP.Crypto.Common;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CFB.IntegrationTests
{
    public class GenValTestCfb64 : GenValTestsCfbBase
    {
        public override string Algorithm { get; } = "TDES";
        public override string Mode { get; } = "CFB64";

        public override AlgoMode AlgoMode => AlgoMode.TDES_CFB64;
    }
}
