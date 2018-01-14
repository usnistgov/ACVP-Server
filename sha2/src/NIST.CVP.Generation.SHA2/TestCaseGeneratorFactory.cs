using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA2
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ISHA _algo;
        private readonly ISHA_MCT _mctAlgo;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, ISHA algo, ISHA_MCT mctAlgo)
        {
            _random800_90 = random800_90;
            _algo = algo;
            _mctAlgo = mctAlgo;
        }
                
        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            if (testGroup.TestType.ToLower() == "aft")
            {
                return new TestCaseGeneratorAFTHash(_random800_90, _algo);
            }
            else if (testGroup.TestType.ToLower() == "mct")
            {
                return new TestCaseGeneratorMCTHash(_random800_90, _mctAlgo);
            }

            return new TestCaseGeneratorNull();
        }
    }
}
