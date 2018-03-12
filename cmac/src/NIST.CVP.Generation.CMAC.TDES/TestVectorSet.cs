using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace NIST.CVP.Generation.CMAC.TDES
{
    public class TestVectorSet : TestVectorSetBase<TestGroup, TestCase>
    {
        public override string Algorithm { get; set; } = "CMAC";
        public override string Mode { get; set; } = "TDES";
    }
}
