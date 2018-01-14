using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.SHA3
{
    public class TestCaseGeneratorSHAKEMCTHash : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ISHA3_MCT _algo;

        public bool IsSample { get; set; } = false;
        private TestCase _seedCaseForTest = null;

        public int NumberOfTestCasesToGenerate { get { return 1; } }

        public TestCaseGeneratorSHAKEMCTHash(IRandom800_90 random800_90, ISHA3_MCT algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public TestCaseGeneratorSHAKEMCTHash(IRandom800_90 random800_90, ISHA3_MCT algo, TestCase seedCase)
        {
            _random800_90 = random800_90;
            _algo = algo;
            _seedCaseForTest = seedCase;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            IsSample = isSample;
            TestCase seedCase;
            if (_seedCaseForTest == null)
            {
                seedCase = new TestCase {Message = _random800_90.GetRandomBitString(group.DigestSize)};
            }
            else
            {
                seedCase = _seedCaseForTest;
            }
            return Generate(group, seedCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            var hashFunction = new HashFunction
            {
                Capacity = group.DigestSize * 2,
                DigestSize = group.DigestSize,
                XOF = true
            };

            MCTResult<AlgoArrayResponse> hashResult = null;
            try
            {
                hashResult = _algo.MCTHash(hashFunction, testCase.Message, group.OutputLength, IsSample);
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
