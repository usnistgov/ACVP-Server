using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IDsaEccFactory _eccDsaFactory;
        private readonly IEccCurveFactory _curveFactory;

        public TestCaseGeneratorFactory(IRandom800_90 rand, IDsaEccFactory eccDsaFactory, IEccCurveFactory curveFactory)
        {
            _random800_90 = rand;
            _eccDsaFactory = eccDsaFactory;
            _curveFactory = curveFactory;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            return new TestCaseGenerator(_random800_90, _eccDsaFactory, _curveFactory);
        }
    }
}
