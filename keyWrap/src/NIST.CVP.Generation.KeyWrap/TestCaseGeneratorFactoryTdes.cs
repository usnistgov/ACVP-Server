using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.KeyWrap;
using NIST.CVP.Crypto.TDES_ECB;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KeyWrap
{
    public class TestCaseGeneratorFactoryTdes : ITestCaseGeneratorFactory<TestGroupTdes, TestCaseTdes>
    {
        private readonly IKeyWrapFactory _iKeyWrapFactory;
        private readonly IRandom800_90 _iRandom800_90;

        public TestCaseGeneratorFactoryTdes(IKeyWrapFactory iKeyWrapFactory, IRandom800_90 iRandom800_90)
        {
            _iKeyWrapFactory = iKeyWrapFactory;
            _iRandom800_90 = iRandom800_90;
        }

        public ITestCaseGenerator<TestGroupTdes, TestCaseTdes> GetCaseGenerator(TestGroupTdes testGroup)
        {
            switch (testGroup.Direction.ToLower())
            {
                case "encrypt":
                    return new TestCaseGeneratorEncryptTdes(_iKeyWrapFactory, _iRandom800_90);
                case "decrypt":
                    return new TestCaseGeneratorDecryptTdes(_iKeyWrapFactory, _iRandom800_90);
                default:
                    return new TestCaseGeneratorNullTdes();
            }
        }
    }
}
