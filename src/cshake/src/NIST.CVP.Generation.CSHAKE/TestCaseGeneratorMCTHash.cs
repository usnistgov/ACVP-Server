using System;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.CSHAKE
{
    public class TestCaseGeneratorMCTHash : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ICSHAKE_MCT _algo;
        private readonly int _stringBitLength = 160;

        public bool IsSample { get; set; } = false;

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGeneratorMCTHash(IRandom800_90 random800_90, ICSHAKE_MCT algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            IsSample = isSample;
            var seedCase = new TestCase
            {
                Message = _random800_90.GetRandomBitString(group.DigestSize)
            };

            return Generate(group, seedCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            var hashFunction = new HashFunction
            {
                Capacity = group.DigestSize * 2,
                DigestSize = group.DigestSize,
                FunctionName = testCase.FunctionName,
                Customization = testCase.Customization
            };

            MCTResult<AlgoArrayResponseWithCustomization> hashResult = null;
            try
            {
                hashResult = _algo.MCTHash(hashFunction, testCase.Message, group.OutputLength, IsSample);
                if (!hashResult.Success)
                {
                    ThisLogger.Warn(hashResult.ErrorMessage);
                    return new TestCaseGenerateResponse<TestGroup, TestCase>(hashResult.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
            }

            testCase.ResultsArray = hashResult.Response;
            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
