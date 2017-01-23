using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA1
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ISHA1 _sha1;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, ISHA1 sha1)
        {
            _random800_90 = random800_90;
            _sha1 = sha1;
        }

        // Not much to happen here... Only one possibility
        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator()
        {
            return new TestCaseGeneratorHash(_random800_90, _sha1);
        }
    }
}
