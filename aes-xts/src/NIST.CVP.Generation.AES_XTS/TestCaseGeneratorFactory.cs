using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.AES_XTS;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_XTS
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IAesXts _algo;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, IAesXts algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            var direction = testGroup.Direction.ToLower();

            if (direction == "encrypt")
            {
                return new TestCaseGeneratorEncrypt(_random800_90, _algo);
            }
            else
            {
                return new TestCaseGeneratorDecrypt(_random800_90, _algo);
            }
        }
    }
}
