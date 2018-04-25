using System;
using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen
{
    public class TestVectorSet : ITestVectorSet<TestGroup, TestCase>
    {
        public string Algorithm { get; set; } = "DSA";
        public string Mode { get; set; } = "PQGGen";
        public bool IsSample { get; set; }
        }    }
}
