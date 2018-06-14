using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KMAC
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string TestType { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public List<TestCase> Tests { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}
