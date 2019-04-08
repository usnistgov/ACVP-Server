using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CFB.IntegrationTests
{
    public class GenValTestCfb8 : GenValTestsCfbBase
    {
        public override string Algorithm { get; } = "TDES-CFB8";
        public override string Mode { get; } = string.Empty;

        public override AlgoMode AlgoMode => AlgoMode.TDES_CFB8_v1_0;
    }
}
