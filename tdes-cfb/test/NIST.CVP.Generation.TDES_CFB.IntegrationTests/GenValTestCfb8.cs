using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CFB.IntegrationTests
{
    public class GenValTestCfb8 : GenValTestsCfbBase
    {
        public override string Algorithm { get; } = "TDES";
        public override string Mode { get; } = "CFB8";

        public override string RunnerAlgorithm => "TDES";
        public override string RunnerMode => "CFB64";

        [SetUp]
        public override void SetUp()
        {
            AdditionalParameters = new[] { "TDES-CFB8" }; 
            GenValApp.Helpers.AutofacConfig.OverrideRegistrations = null;
        }
    }
}
