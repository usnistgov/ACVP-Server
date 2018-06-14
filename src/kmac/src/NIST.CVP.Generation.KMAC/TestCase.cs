using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KMAC
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public TestGroup ParentGroup { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public bool? TestPassed => throw new System.NotImplementedException();

        public bool Deferred => throw new System.NotImplementedException();
    }
}
