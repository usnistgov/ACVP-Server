using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.FFC.SigVer
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IDsaFfcFactory _dsaFactory;

        public TestCaseGeneratorFactory(IRandom800_90 rand, IDsaFfcFactory dsaFactory)
        {
            _random800_90 = rand;
            _dsaFactory = dsaFactory;
       }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            return new TestCaseGenerator(_random800_90, _dsaFactory);
        }
    }
}
