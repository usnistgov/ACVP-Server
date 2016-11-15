using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IAES_GCM _aesGcm;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, IAES_GCM aesGcm)
        {
            _aesGcm = aesGcm;
            _random800_90 = random800_90;
        }

        public ITestCaseGenerator GetCaseGenerator(string direction, string ivGen)
        {
            direction = direction.ToLower();
            ivGen = ivGen.ToLower();
            if (direction == "encrypt")
            {
                if (ivGen == "internal")
                {
                    return new TestCaseGeneratorInternalEncrypt(_random800_90, _aesGcm);
                }
                if (ivGen == "external")
                {
                    return new TestCaseGeneratorExternalEncrypt(_random800_90, _aesGcm);
                }
            }

            if (direction == "decrypt")
            {
                return new TestCaseGeneratorDecrypt(_random800_90, _aesGcm);
            }
            return new TestCaseGeneratorNull();
        }
    }
}
