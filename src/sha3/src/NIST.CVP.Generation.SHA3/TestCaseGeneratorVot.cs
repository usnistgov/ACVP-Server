using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Generation.SHA3
{
    public class TestCaseGeneratorVot : ITestCaseGenerator<TestGroup, TestCase>
    {
        private int _capacity = 0;
        private int _currentCase = 0;
        private int _digestSize = 0;
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => TestCaseSizes.Count;
        public List<int> TestCaseSizes { get; } = new List<int>();                 // Primarily for testing purposes

        public TestCaseGeneratorVot(IOracle oracle)
        {
            _oracle = oracle;
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

            var param = new Sha3Parameters
            {
                HashFunction = new HashFunction(_digestSize, _capacity, true),
                MessageLength = _capacity / 2
            };

            Common.Oracle.ResultTypes.HashResult oracleResult = null;
            try
            {
                oracleResult = _oracle.GetSha3Case(param);
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }

            return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
            {
                Message = oracleResult.Message,
                Digest = oracleResult.Digest
            });
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            return null;
        }

        private void DetermineLengths(MathDomain domain)
        {
            domain.SetRangeOptions(RangeDomainSegmentOptions.Random);
            var minMax = domain.GetDomainMinMax();

            var values = domain.GetValues(1000).OrderBy(o => Guid.NewGuid()).Take(1000);
            int repetitions;

            if (!values.Any())
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



