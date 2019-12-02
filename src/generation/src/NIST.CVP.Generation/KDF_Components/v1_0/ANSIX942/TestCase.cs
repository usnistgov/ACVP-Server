using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.KDF_Components.v1_0.ANSIX942
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }

        public BitString Zz { get; set; }
        public BitString DerivedKey { get; set; }
        public BitString OtherInfo { get; set; }
    }
}
