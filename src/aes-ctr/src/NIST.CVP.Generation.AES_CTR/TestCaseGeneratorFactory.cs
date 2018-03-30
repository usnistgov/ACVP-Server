using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.AES_CTR;
using NIST.CVP.Crypto.Common.Symmetric.AES;
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

            switch (testType)
            {
                case "gfsbox":
                case "keysbox":
                case "vartxt":
                case "varkey":
                    return new TestCaseGeneratorKnownAnswer(group.KeyLength, testType);
                case "singleblock":
                    switch (direction)
                    {
                        case "encrypt":
                            return new TestCaseGeneratorSingleBlockEncrypt(_rand, _algo);
                        case "decrypt":
                            return new TestCaseGeneratorSingleBlockDecrypt(_rand, _algo);
                    }

                    break;
                case "partialblock":
                    switch (direction)
                    {
                        case "encrypt":
                            return new TestCaseGeneratorPartialBlockEncrypt(_rand, _algo);
                        case "decrypt":
                            return new TestCaseGeneratorPartialBlockDecrypt(_rand, _algo);
                    }

                    break;
                case "counter":
                    switch (direction)
                    {
                        case "encrypt":
                            return new TestCaseGeneratorCounterEncrypt(_rand, _algo);
                        case "decrypt":
                            return new TestCaseGeneratorCounterDecrypt(_rand, _algo);
                    }

                    break;
            }

            return new TestCaseGeneratorNull();
        }
    }
}
