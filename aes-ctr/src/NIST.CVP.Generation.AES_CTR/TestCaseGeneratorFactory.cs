using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.AES_CTR;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CTR
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly IAesCtr _algo;

        public TestCaseGeneratorFactory(IRandom800_90 rand, IAesCtr algo)
        {
            _rand = rand;
            _algo = algo;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup group)
        {
            var direction = group.Direction.ToLower();
            var testType = group.TestType.ToLower();

            if (testType == "singleblock")
            {
                if (direction == "encrypt")
                {
                    return new TestCaseGeneratorSingleBlockEncrypt(_rand, _algo);
                }
                else if (direction == "decrypt")
                {
                    return new TestCaseGeneratorSingleBlockDecrypt(_rand, _algo);
                }
            }
            else if (testType == "partialblock")
            {
                if (direction == "encrypt")
                {
                    return new TestCaseGeneratorPartialBlockEncrypt(_rand, _algo);
                }
                else if (direction == "decrypt")
                {
                    return new TestCaseGeneratorPartialBlockDecrypt(_rand, _algo);
                }
            }
            else if (testType == "counter")
            {
                if (direction == "encrypt")
                {
                    
                }
                else if (direction == "decrypt")
                {
                    
                }
            }

            return new TestCaseGeneratorNull();
        }
    }
}
