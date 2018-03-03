using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Generation.SHA3
{
    public class TestCaseGeneratorSHAKEVOTHash : ITestCaseGenerator<TestGroup, TestCase>
    {
        private int _capacity = 0;
        private int _currentCase = 0;
        private int _digestSize = 0;
        private List<int> _testCaseSizes = new List<int>();

        private readonly IRandom800_90 _random800_90;
        private readonly ISHA3 _algo;

        public int NumberOfTestCasesToGenerate { get { return _testCaseSizes.Count; } }
        public List<int> TestCaseSizes { get { return _testCaseSizes; } }                   // Primarily for testing purposes

        public TestCaseGeneratorSHAKEVOTHash(IRandom800_90 random800_90, ISHA3 algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
            _testCaseSizes.Add(-1);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            // Only do this logic once
            if (_capacity == 0)
            {
                _testCaseSizes.Clear();
                DetermineLengths(group.OutputLength);
                _capacity = 2 * group.DigestSize;
            }

            _digestSize = _testCaseSizes[_currentCase];
            _currentCase++;

            var message = _random800_90.GetRandomBitString(_capacity / 2);
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
                    Capacity = _capacity,
                    DigestSize = _digestSize,
                    XOF = true
                };

                hashResult = _algo.HashMessage(hashFunction, testCase.Message);
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

            testCase.Digest = hashResult.Digest;
            return new TestCaseGenerateResponse(testCase);
        }

        private void DetermineLengths(MathDomain domain)
        {
            domain.SetRangeOptions(RangeDomainSegmentOptions.Random);
            var minMax = domain.GetDomainMinMax();

            var values = domain.GetValues(1000).OrderBy(o => Guid.NewGuid()).Take(1000);
            int repetitions;

            if (values.Count() == 0)
            {
                repetitions = 999;
            }
            else if(values.Count() > 999)
            {
                repetitions = 1;
            }
            else
            {
                repetitions = 1000 / values.Count() + (1000 % values.Count() > 0 ? 1 : 0);
            }

            foreach(var value in values)
            {
                for(var i = 0; i < repetitions; i++)
                {
                    _testCaseSizes.Add(value);
                }
            }

            // Make sure min and max appear in the list
            _testCaseSizes.Add(minMax.Minimum);
            _testCaseSizes.Add(minMax.Maximum);

            _testCaseSizes.Sort();
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}



