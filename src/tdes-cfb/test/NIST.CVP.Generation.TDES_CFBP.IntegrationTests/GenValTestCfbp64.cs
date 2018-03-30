using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CFBP.IntegrationTests
{
    public class GenValTestCfbp64 : GenValTestsCfbpBase
    {
        public override string Algorithm { get; } = "TDES";
        public override string Mode { get; } = "CFBP64";

        public override AlgoMode AlgoMode => AlgoMode.TDES_CFBP64;
    }
}
