using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IDsaEcc _eccDsa;

        public TestCaseGeneratorFactory(IRandom800_90 rand)
        {
            _random800_90 = rand;
            _eccDsa = new EccDsa(null);     // Don't need a hash function
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            return new TestCaseGenerator(_random800_90, _eccDsa);
        }
    }
}
