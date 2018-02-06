using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CFBP.IntegrationTests
{
    public class GenValTestCfbp8 : GenValTestsCfbpBase
    {
        public override string Algorithm { get; } = "TDES-CFBP8";
        public override string Mode { get; } = "CFBP8";

        public override string RunnerAlgorithm => "TDES";
        public override string RunnerMode => "CFBP8";

        [SetUp]
        public override void SetUp()
        {
            AdditionalParameters = new[] { "TDES-CFBP8" }; 
            GenValApp.Helpers.AutofacConfig.OverrideRegistrations = null;
        }
    }
}
