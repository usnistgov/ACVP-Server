using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;

namespace NIST.CVP.Generation.CMAC.AES
{
    public class TestVectorSet : TestVectorSetBase<TestGroup, TestCase>
    {
        public override string Algorithm { get; set; } = "CMAC";
        public override string Mode { get; set; } = "AES";
    }
}
