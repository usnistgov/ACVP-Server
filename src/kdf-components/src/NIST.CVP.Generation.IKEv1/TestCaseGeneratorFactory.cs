using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.KDF.Components.IKEv1;
using NIST.CVP.Crypto.IKEv1;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.IKEv1
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IIkeV1Factory _ikeFactory;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, IIkeV1Factory factory)
        {
            _random800_90 = random800_90;
            _ikeFactory = factory;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            var ike = _ikeFactory.GetIkeV1Instance(testGroup.AuthenticationMethod, testGroup.HashAlg);
            return new TestCaseGenerator(_random800_90, ike);
        }
    }
}
