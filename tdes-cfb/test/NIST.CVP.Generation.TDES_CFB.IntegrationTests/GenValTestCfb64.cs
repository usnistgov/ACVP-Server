using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CFB.IntegrationTests
{
    public class GenValTestCfb64 : GenValTestsCfbBase
    {
        public override string Algorithm { get; } = "TDES";
        public override string Mode { get; } = "CFB64";

        public override string RunnerAlgorithm => "TDES";
        public override string RunnerMode => "CFB64";

        [SetUp]
        public override void SetUp()
        {
            AdditionalParameters = new[] { "TDES-CFB64" }; 
            GenValApp.Helpers.AutofacConfig.OverrideRegistrations = null;
        }
    }
}
