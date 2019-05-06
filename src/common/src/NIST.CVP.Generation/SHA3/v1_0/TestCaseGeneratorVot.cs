using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Generation.SHA3.v1_0
{
    public class TestCaseGeneratorVot : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private int _capacity = 0;
        private int _currentCase = 0;
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => TestCaseSizes.Count;
        public List<int> TestCaseSizes { get; } = new List<int>();                 // Public for testing purposes

        public TestCaseGeneratorVot(IOracle oracle)
        {
            _oracle = oracle;
        }
        
        public GenerateResponse PrepareGenerator(TestGroup @group, bool isSample)
        {
            DetermineLengths(group.OutputLength);
            _capacity = 2 * group.DigestSize;
            
            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new Sha3Parameters
            {
                HashFunction = new HashFunction(TestCaseSizes[caseNo], _capacity, true),
                MessageLength = _capacity / 2
            };

            try
            {
                var oracleResult = await _oracle.GetSha3CaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Message = oracleResult.Message,
                    Digest = oracleResult.Digest
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        private void DetermineLengths(MathDomain domain)
        {
            domain.SetRangeOptions(RangeDomainSegmentOptions.Random);
            var minMax = domain.GetDomainMinMax();

            var values = domain.GetValues(1000).OrderBy(o => Guid.NewGuid()).Take(1000).ToList();
            int repetitions;
            
            if(values.Count > 999)
            {
                repetitions = 1;
            }
            else
            {
                repetitions = 1000 / values.Count + (1000 % values.Count > 0 ? 1 : 0);
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

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}



