using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA1
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ISHA1 _sha1;
        private readonly ISHA1_MCT _sha1_mct;
        public const int _DEFAULT_NUMBER_TEST_CASES = 15;

        public int NumberOfTestCases { get; private set; } = _DEFAULT_NUMBER_TEST_CASES;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, ISHA1 sha1, ISHA1_MCT sha1_mct)
        {
            _random800_90 = random800_90;
            _sha1 = sha1;
            _sha1_mct = sha1_mct;
        }

        // Not much to happen here... Only one possibility
        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            var testType = testGroup.TestType.ToLower();

            switch (testType)
            {
                case "mct":
                    return new TestCaseGeneratorMCTHash(_random800_90, _sha1_mct);
                case "mmt":
                    return new TestCaseGeneratorMMTHash(_random800_90, _sha1);
            }

            return new TestCaseGeneratorNull();
        }
    }
}
