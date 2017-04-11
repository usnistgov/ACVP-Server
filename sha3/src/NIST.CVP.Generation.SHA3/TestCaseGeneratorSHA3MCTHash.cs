using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.SHA3
{
    public class TestCaseGeneratorSHA3MCTHash : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ISHA3_MCT _algo;

        public bool IsSample { get; set; } = false;
        private TestCase _seedCaseForTest = null;

        public int NumberOfTestCasesToGenerate { get { return 1; } }

        public TestCaseGeneratorSHA3MCTHash(IRandom800_90 random800_90, ISHA3_MCT algo, bool isSample)
        {
            _random800_90 = random800_90;
            _algo = algo;
            IsSample = isSample;
        }

        public TestCaseGeneratorSHA3MCTHash(IRandom800_90 random800_90, ISHA3_MCT algo, TestCase seedCase)
        {
            _random800_90 = random800_90;
            _algo = algo;
            _seedCaseForTest = seedCase;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            IsSample = isSample;
            var seedCase = new TestCase { Message = _random800_90.GetRandomBitString(group.DigestSize), Deferred = false };
            return Generate(group, seedCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            var hashFunction = new HashFunction
            {
                Capacity = group.DigestSize * 2,
                DigestSize = group.DigestSize,
                XOF = false
            };

            MCTResult<AlgoArrayResponse> hashResult = null;
            try
            {
                hashResult = _algo.MCTHash(hashFunction, testCase.Message, IsSample, 0, 0);
                if (!hashResult.Success)
                {
                    ThisLogger.Warn(hashResult.ErrorMessage);
                    return new TestCaseGenerateResponse(hashResult.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse(ex.Message);
            }

            testCase.ResultsArray = hashResult.Response;
            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}
