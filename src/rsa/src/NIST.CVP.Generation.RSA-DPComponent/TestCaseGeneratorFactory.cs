using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA_DPComponent
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IKeyBuilder _keyBuilder;
        private readonly IRsa _rsa;
        private readonly IKeyComposerFactory _keyComposerFactory;

        public TestCaseGeneratorFactory(IRandom800_90 random, IKeyBuilder keyBuilder, IRsa rsa, IKeyComposerFactory keyComposerFactory)
        {
            _random800_90 = random;
            _keyBuilder = keyBuilder;
            _rsa = rsa;
            _keyComposerFactory = keyComposerFactory;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup group)
        {
            return new TestCaseGenerator(_random800_90, _keyBuilder, _keyComposerFactory, _rsa);
        }
    }
}
