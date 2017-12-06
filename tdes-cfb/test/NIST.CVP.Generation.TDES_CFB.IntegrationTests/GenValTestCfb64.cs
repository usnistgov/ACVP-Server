using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CFB.IntegrationTests
{
    public class GenValTestCfb64 : GenValTestsCfbBase
    {
        public override string Algorithm { get; } = "TDES-CFB64";
        public override string Mode { get; } = "CFB64";

        [SetUp]
        public override void SetUp()
        {
            AdditionalParameters = new[] { "TDES-CFB64" }; 
            AutofacConfig.OverrideRegistrations = null;
            TDES_CFB_Val.AutofacConfig.OverrideRegistrations = null;
        }
    }
}
