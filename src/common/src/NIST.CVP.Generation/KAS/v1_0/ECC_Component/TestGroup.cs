using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS.v1_0.ECC_Component
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        public Curve Curve { get; set; }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();
    }
}