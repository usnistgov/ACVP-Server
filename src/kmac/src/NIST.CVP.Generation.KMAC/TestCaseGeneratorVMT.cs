using System.Linq;
using NIST.CVP.Crypto.Common.MAC;
using NIST.CVP.Crypto.Common.MAC.KMAC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NLog;
using System.Collections.Generic;
using System;

namespace NIST.CVP.Generation.KMAC
{
    public class TestCaseGeneratorVMT : ITestCaseGenerator<TestGroup, TestCase>
    {
        private int _capacity = 0;
        private int _currentCase = 0;
        private int _macLength = 0;
        private readonly IRandom800_90 _random800_90;
        private readonly IKmac _algo;

        public int NumberOfTestCasesToGenerate => TestCaseSizes.Count;
        public List<int> TestCaseSizes { get; } = new List<int>();                 // Primarily for testing purposes

        public TestCaseGeneratorVMT(IRandom800_90 random800_90, IKmac algo)
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
                DetermineLengths(group.MacLengths);
                _capacity = 2 * group.DigestSize;
            }

            _macLength = TestCaseSizes[_currentCase];
            _currentCase++;

            var message = _random800_90.GetRandomBitString(group.MessageLength);
            var key = _random800_90.GetRandomBitString(group.KeyLength);
            var customization = _random800_90.GetRandomString(_random800_90.GetRandomInt(1, 21));

            var testCase = new TestCase
            {
                Key = key,
                Message = message,
                Customization = customization
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            MacResult genResult = null;
            try
            {
                genResult = _algo.Generate(testCase.Key, testCase.Message, testCase.Customization, _macLength);
                if (!genResult.Success)
                {
                    ThisLogger.Warn(genResult.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse<TestGroup, TestCase>(genResult.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                {
                    return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
                }
            }
            testCase.Mac = genResult.Mac;

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
            else if (values.Count() > 999)
            {
                repetitions = 1;
            }
            else
            {
                repetitions = 1000 / values.Count() + (1000 % values.Count() > 0 ? 1 : 0);
            }

            foreach (var value in values)
            {
                for (var i = 0; i < repetitions; i++)
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
