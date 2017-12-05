using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using NIST.CVP.Crypto.TDES_CTR;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES_CTR
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ITdesCtr _algo;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, ITdesCtr algo)
        {
            _algo = algo;
            _random800_90 = random800_90;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup group)
        {
            var testType = group.TestType.ToLower();
            var direction = group.Direction.ToLower();

            if (testType == "singleblock")
            {
                if (direction == "encrypt")
                {
                    return new TestCaseGeneratorSingleBlockEncrypt(_random800_90, _algo);
                }

                if (direction == "decrypt")
                { 
                    return new TestCaseGeneratorSingleBlockDecrypt(_random800_90, _algo);
                }
            }
            else if (testType == "partialblock")
            {
                if (direction == "encrypt")
                {
                    return new TestCaseGeneratorPartialBlockEncrypt(_random800_90, _algo);
                }

                if (direction == "decrypt")
                {
                    return new TestCaseGeneratorPartialBlockDecrypt(_random800_90, _algo);
                }
            }   
            else if (testType == "counter")
            {
                if (direction == "encrypt")
                {
                    return new TestCaseGeneratorCounterEncrypt(_random800_90, _algo);
                }

                if (direction == "decrypt")
                {
                    return new TestCaseGeneratorCounterDecrypt(_random800_90, _algo);
                }
            }

            return new TestCaseGeneratorNull();
        }
    }
}
