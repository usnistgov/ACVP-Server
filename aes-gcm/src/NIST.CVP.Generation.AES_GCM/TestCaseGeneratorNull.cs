using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestCaseGeneratorNull : ITestCaseGenerator
    {
      
        public string Direction
        {
            get { return "None"; }
        }

        public string IVGen
        {
            get { return "None"; }
        }

        public TestCaseGenerateResponse Generate(TestGroup @group)
        {
            return Generate(group, null);
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, TestCase testCase)
        {
            return new TestCaseGenerateResponse("This is the null generator -- nothing is generated");
        }
    }
}
