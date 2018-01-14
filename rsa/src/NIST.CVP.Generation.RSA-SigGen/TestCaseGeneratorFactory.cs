using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.RSA.Signatures;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;

namespace NIST.CVP.Generation.RSA_SigGen
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
            if (testGroup.TestType.ToLower() == "gdt")
            {
                switch (testGroup.Mode)
                {
                    case SigGenModes.ANS_931:
                        return new TestCaseGeneratorGDT(_random800_90, new ANS_X931_Signer(testGroup.HashAlg));

                    case SigGenModes.PKCS_v15:
                        return new TestCaseGeneratorGDT(_random800_90, new RSASSA_PKCSv15_Signer(testGroup.HashAlg));

                    case SigGenModes.PSS:
                        return new TestCaseGeneratorGDT(_random800_90, new RSASSA_PSS_Signer(testGroup.HashAlg, Math.Entropy.EntropyProviderTypes.Testable, testGroup.SaltLen));
                }
            }

            return new TestCaseGeneratorNull();
        }
    }
}
