using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.KeyGen
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; } = "AFT";

        public LmsMode LmsMode { get; set; }
        public LmOtsMode LmOtsMode { get; set; }

        public List<TestCase> Tests { get; set; } = new();
    }
}
