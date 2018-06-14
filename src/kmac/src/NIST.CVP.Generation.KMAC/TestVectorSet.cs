using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KMAC
{
    public class TestVectorSet : ITestVectorSet<TestGroup, TestCase>
    {
        public string Algorithm { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string Mode { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public bool IsSample { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public List<TestGroup> TestGroups { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}
