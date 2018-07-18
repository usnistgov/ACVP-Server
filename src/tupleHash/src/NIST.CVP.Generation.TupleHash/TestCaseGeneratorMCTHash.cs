using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Hash.TupleHash;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.TupleHash
{
    public class TestCaseGeneratorMCTHash : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ITupleHash_MCT _algo;

        public bool IsSample { get; set; } = false;

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGeneratorMCTHash(IRandom800_90 random800_90, ITupleHash_MCT algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            IsSample = isSample;
            var seedCase = new TestCase
            {
                Tuple = new List<BitString>(new BitString[] { _random800_90.GetRandomBitString(144) })  // always 144 for the length
            };

            return Generate(group, seedCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            var hashFunction = new HashFunction
            {
                Capacity = group.DigestSize * 2,
                DigestLength = group.DigestSize,
                XOF = group.XOF,
                Customization = testCase.Customization
            };

            MCTResultTuple<AlgoArrayResponse> hashResult = null;
            try
            {
                hashResult = _algo.MCTHash(hashFunction, testCase.Tuple, group.OutputLength, IsSample);
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
