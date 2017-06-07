using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.SHA2
{
    public class TestCaseGeneratorAFTHash : ITestCaseGenerator<TestGroup, TestCase>
    {
        private int _numberOfCases = 512;
        private int _currentSmallCase = 0;
        private int _currentLargeCase = 1;

        private readonly IRandom800_90 _random800_90;
        private readonly ISHA _algo;

        public int NumberOfTestCasesToGenerate { get { return _numberOfCases; } }

        public TestCaseGeneratorAFTHash(IRandom800_90 random800_90, ISHA algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            var unitSize = group.BitOriented ? 1 : 8;
            var blockSize = SHAEnumHelpers.DetermineBlockSize(group.DigestSize);

            var numSmallCases = blockSize / unitSize;
            var numLargeCases = blockSize / unitSize;

            if (!group.IncludeNull)
            {
                if (_currentSmallCase == 0)
                {
                    _currentSmallCase = 1;
                }
            }
            else
            {
                numSmallCases = blockSize / unitSize + 1;
            }

            _numberOfCases = numSmallCases + numLargeCases;

            var message = new BitString(0);
            if (_currentSmallCase <= numSmallCases)
            {
                message = _random800_90.GetRandomBitString(unitSize * _currentSmallCase);
                _currentSmallCase++;
            }
            else
            {
                message = _random800_90.GetRandomBitString(blockSize + (unitSize * 99 * _currentLargeCase));
                _currentLargeCase++;
            }

            var testCase = new TestCase
            {
                Message = message,
                Deferred = false
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            HashResult hashResult = null;

            try
            {
                var hashFunction = new HashFunction
                {
                    Mode = group.Function,
                    DigestSize = group.DigestSize
                };

                hashResult = _algo.HashMessage(hashFunction, testCase.Message);
                if (!hashResult.Success)
                {
                    ThisLogger.Warn(hashResult.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse(hashResult.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                {
                    return new TestCaseGenerateResponse(ex.Message);
                }
            }

            testCase.Digest = hashResult.Digest;
            return new TestCaseGenerateResponse(testCase);
        }

        public List<TestCaseGenerateResponse> GenerateInParallel(TestGroup group, bool isSample, int startId)
        {
            var responses = new List<TestCaseGenerateResponse>();

            // Determine size of each test
            var values = GetListOfSizes(group, isSample);

            for (var i = 0; i < values.Count; i++)
            {
                var response = GenerateOfSize(group, values[i], startId + i);
                responses.Add(response);
            }

            // Run test generation
            //Parallel.For(0, values.Count, new ParallelOptions {MaxDegreeOfParallelism = 2},
            //    i =>
            //    {
            //        var response = GenerateOfSize(group, values[i], startId + i);
            //        responses.Add(response);
            //    });

            // Waits til completion before returning
            return responses;
        }

        private TestCaseGenerateResponse GenerateOfSize(TestGroup group, int bitLen, int tcId)
        {
            var message = _random800_90.GetRandomBitString(bitLen);
            var testCase = new TestCase
            {
                TestCaseId = tcId,
                Message = message,
                Deferred = false
            };

            return Generate(group, testCase);
        }

        private List<int> GetListOfSizes(TestGroup group, bool isSample)
        {
            var sizes = new List<int>();
            var unitSize = group.BitOriented ? 1 : 8;
            var blockSize = SHAEnumHelpers.DetermineBlockSize(group.DigestSize);

            var numSmallCases = blockSize / unitSize;
            var numLargeCases = blockSize / unitSize;

            if (group.IncludeNull)
            {
                sizes.Add(0);
            }

            _numberOfCases = numSmallCases + numLargeCases + (group.IncludeNull ? 1 : 0);

            for (var i = 1; i <= numSmallCases; i++)
            {
                sizes.Add(unitSize * i);
            }

            for (var i = 1; i <= numLargeCases; i++)
            {
                sizes.Add(blockSize + unitSize * 99 * i);
            }

            return sizes;
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}
