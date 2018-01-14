using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.FFC.SigGen
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IShaFactory _shaFactory;
        private IDsaFfc _ffcDsa;

        public TestCaseGeneratorFactory(IRandom800_90 rand)
        {
            _random800_90 = rand;
            _shaFactory = new ShaFactory();
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            _ffcDsa = new FfcDsa(_shaFactory.GetShaInstance(testGroup.HashAlg));
            return new TestCaseGenerator(_random800_90, _ffcDsa);
        }
    }
}
