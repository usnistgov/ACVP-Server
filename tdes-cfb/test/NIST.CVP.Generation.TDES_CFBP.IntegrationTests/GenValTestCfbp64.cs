using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CFBP.IntegrationTests
{
    public class GenValTestCfbp64 : GenValTestsCfbpBase
    {
        public override string Algorithm { get; } = "TDES-CFBP64";
        public override string Mode { get; } = "CFBP64";

        public override string RunnerAlgorithm => "TDES";
        public override string RunnerMode => "CFBP64";

        [SetUp]
        public override void SetUp()
        {
            AdditionalParameters = new[] { "TDES-CFBP64" }; 
            GenValApp.Helpers.AutofacConfig.OverrideRegistrations = null;
        }
    }
}
