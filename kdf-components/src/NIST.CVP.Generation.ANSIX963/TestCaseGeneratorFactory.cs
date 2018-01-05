using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.ANSIX963;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.ANSIX963
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90)
        {
            _random800_90 = random800_90;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            var sha = new ShaFactory().GetShaInstance(testGroup.HashAlg);
            return new TestCaseGenerator(_random800_90, new AnsiX963(sha));
        }
    }
}
