using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
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

            switch (testType)
            {
                case "permutation":
                case "inversepermutation":
                case "substitutiontable":
                case "variablekey":
                case "variabletext":
                    return new TestCaseGeneratorKnownAnswer(group);
                case "singleblock":
                    switch (direction)
                    {
                        case "encrypt":
                            return new TestCaseGeneratorSingleBlockEncrypt(_random800_90, _algo);
                        case "decrypt":
                            return new TestCaseGeneratorSingleBlockDecrypt(_random800_90, _algo);
                    }

                    break;
                case "partialblock":
                    switch (direction)
                    {
                        case "encrypt":
                            return new TestCaseGeneratorPartialBlockEncrypt(_random800_90, _algo);
                        case "decrypt":
                            return new TestCaseGeneratorPartialBlockDecrypt(_random800_90, _algo);
                    }

                    break;
                case "counter":
                    switch (direction)
                    {
                        case "encrypt":
                            return new TestCaseGeneratorCounterEncrypt(_random800_90, _algo);
                        case "decrypt":
                            return new TestCaseGeneratorCounterDecrypt(_random800_90, _algo);
                    }

                    break;
            }

            return new TestCaseGeneratorNull();
        }
    }
}
