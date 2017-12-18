using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.KDF;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KDF
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IKdfFactory _kdfFactory;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, IKdfFactory factory)
        {
            _random800_90 = random800_90;
            _kdfFactory = factory;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            var kdf = _kdfFactory.GetKdfInstance(testGroup.KdfMode, testGroup.MacMode, testGroup.CounterLocation, testGroup.CounterLength);
            return new TestCaseGenerator(_random800_90, kdf);
        }
    }
}
