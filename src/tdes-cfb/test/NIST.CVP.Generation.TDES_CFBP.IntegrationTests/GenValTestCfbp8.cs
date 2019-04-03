using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common;
using NIST.CVP.Crypto.Common;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CFBP.IntegrationTests
{
    public class GenValTestCfbp8 : GenValTestsCfbpBase
    {
        public override string Algorithm { get; } = "TDES-CFBP8";
        public override string Mode { get; } = string.Empty;

        public override AlgoMode AlgoMode => AlgoMode.TDES_CFBP8_v1_0;
    }
}
