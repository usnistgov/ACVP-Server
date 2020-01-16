using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.SafePrimeGroups.v1_0.KeyVer
{
    public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly  List<SafePrimesKeyDisposition> _testDispositions;
        
        public int NumberOfTestCasesToGenerate { get; }

        public TestCaseGenerator(IOracle oracle, List<SafePrimesKeyDisposition> validityTestCaseOptions)
        {
            _oracle = oracle;
            _testDispositions = validityTestCaseOptions;
            NumberOfTestCasesToGenerate = _testDispositions.Count;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new SafePrimesKeyVerParameters()
            {
                SafePrime = group.SafePrimeGroup,
                DomainParameters = group.DomainParameters,
                Disposition = TestCaseDispositionHelper.GetTestCaseIntention(_testDispositions)
            };

            try
            {
                var result = await _oracle.GetSafePrimesKeyVerTestAsync(param);

                var testCase = new TestCase
                {
                    Key = result.KeyPair,
                    TestPassed = result.TestPassed
                };

                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating key: {ex.Message}");
            }
        }
        
        private static readonly ILogger ThisLogger = LogManager.GetCurrentClassLogger();
    }
}