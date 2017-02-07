using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.SHA;
using NIST.CVP.Generation.SHA2;
using NIST.CVP.Math;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA2
{
    public class TestCaseGeneratorShortHash : ITestCaseGenerator<TestGroup, TestCase>
    {
        private int _numberOfCases = 512;
        private int _currentCase = 0;

        private readonly IRandom800_90 _random800_90;
        private readonly ISHA _algo;

        public int NumberOfTestCasesToGenerate { get { return _numberOfCases; } }

        public TestCaseGeneratorShortHash(IRandom800_90 random800_90, ISHA algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            var unitSize = (group.BitOriented ? 1 : 8);
            _numberOfCases = DetermineBlockSize(group.DigestSize) / unitSize;

            var message = new BitString(0);

            if (group.IncludeNull)
            {
                if(_currentCase == 0)
                {
                    _numberOfCases++;
                }
            }else
            {
                message = _random800_90.GetRandomBitString(unitSize * (_currentCase + 1));
            }

            var testCase = new TestCase
            {
                Message = message,
                Deferred = false
            };

            _currentCase++;

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

        private int DetermineBlockSize(DigestSizes digestSize)
        {
            switch (digestSize)
            {
                case DigestSizes.d160:
                case DigestSizes.d224:
                case DigestSizes.d256:
                    return 512;

                case DigestSizes.d384:
                case DigestSizes.d512:
                case DigestSizes.d512t224:
                case DigestSizes.d512t256:
                    return 1024;
            }

            throw new Exception("Invalid block size in TestCaseGenerator");
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}
