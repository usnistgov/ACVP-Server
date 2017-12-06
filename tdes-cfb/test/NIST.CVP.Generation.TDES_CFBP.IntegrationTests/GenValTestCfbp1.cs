using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CFBP.IntegrationTests
{
    public class GenValTestCfbp1 : GenValTestsCfbpBase
    {
        public override string Algorithm { get; } = "TDES-CFBP1";
        public override string Mode { get; } = "CFBP1";

        [SetUp]
        public override void SetUp()
        {
            AdditionalParameters = new[] { "TDES-CFBP1" }; 
            AutofacConfig.OverrideRegistrations = null;
            TDES_CFB_Val.AutofacConfig.OverrideRegistrations = null;
        }
    }
}
