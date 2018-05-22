using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Helpers;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestVectorSet: ITestVectorSet<TestGroup, TestCase>
    {
        public string Algorithm { get; set; } = "AES";
        public string Mode { get; set; } = "GCM";
        public bool IsSample { get; set; }
        public List<TestGroup> TestGroups { get; set; } = new List<TestGroup>();
    }
}
