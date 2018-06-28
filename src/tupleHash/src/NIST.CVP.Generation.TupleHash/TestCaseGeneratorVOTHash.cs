using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.TupleHash;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Generation.TupleHash
{
    public class TestCaseGeneratorVOTHash : ITestCaseGenerator<TestGroup, TestCase>
    {
        private int _capacity = 0;
        private int _currentCase = 0;
        private int _digestSize = 0;
        private readonly IRandom800_90 _random800_90;
        private readonly ITupleHash _algo;

        public int NumberOfTestCasesToGenerate => TestCaseSizes.Count;
        public List<int> TestCaseSizes { get; } = new List<int>();                 // Primarily for testing purposes

        public TestCaseGeneratorVOTHash(IRandom800_90 random800_90, ITupleHash algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
            TestCaseSizes.Add(-1);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            // Only do this logic once
            if (_capacity == 0)
            {
                TestCaseSizes.Clear();
                DetermineLengths(group.OutputLength);
                _capacity = 2 * group.DigestSize;
            }

            _digestSize = TestCaseSizes[_currentCase];
            _currentCase++;

            var tuple = new List<BitString>();
            for (int i = 0; i < 2; i++)
            {
                tuple.Add(_random800_90.GetRandomBitString(_capacity / 2));
            }
            var customization = _random800_90.GetRandomString(_random800_90.GetRandomInt(0, 21));
            
            var testCase = new TestCase
            {
                Tuple = tuple,
                Customization = customization,
                Deferred = false
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            HashResult hashResult = null;

            try
            {
                var hashFunction = new HashFunction
                {
                    Capacity = _capacity,
                    DigestSize = _digestSize,
                    XOF = group.XOF,
                    Customization = testCase.Customization
                };

                hashResult = _algo.HashMessage(hashFunction, testCase.Tuple);
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

            testCase.Digest = hashResult.Digest;
            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
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
                    TestCaseSizes.Add(value);
                }
            }

            // Make sure min and max appear in the list
            TestCaseSizes.Add(minMax.Minimum);
            TestCaseSizes.Add(minMax.Maximum);

            TestCaseSizes.Sort();
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}



