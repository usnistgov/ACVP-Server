using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CFB8
{
    public class TestVectorSet : ITestVectorSet<TestGroup, TestCase>
    {
        public string Algorithm { get; set; } = "AES";
        public string Mode { get; set; } = "CFB8";
        public bool IsSample { get; set; }
        public List<TestGroup> TestGroups { get; set; } = new List<TestGroup>();
    }
}
