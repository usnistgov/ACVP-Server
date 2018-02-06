using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CFB.IntegrationTests
{
    public class GenValTestCfb1 : GenValTestsCfbBase
    {
        public override string Algorithm { get; } = "TDES-CFB1";
        public override string Mode { get; } = "CFB1";

        public override string RunnerAlgorithm => "TDES";
        public override string RunnerMode => "CFB1";

        [SetUp]
        public override void SetUp()
        {
            AdditionalParameters = new[] { "TDES-CFB1" };
            GenValApp.Helpers.AutofacConfig.OverrideRegistrations = null;
        }
    }
}
