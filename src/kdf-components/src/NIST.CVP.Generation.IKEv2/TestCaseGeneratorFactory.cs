using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.IKEv2;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.IKEv2
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
            var hmac = new HmacFactory(new ShaFactory()).GetHmacInstance(testGroup.HashAlg);
            return new TestCaseGenerator(_random800_90, new IkeV2(hmac));
        }
    }
}
