using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.SafePrimeGroups.v1_0.KeyVer.TestCaseExpectations;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.SafePrimeGroups.v1_0.KeyVer
{
    public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly KeyExpectationProvider _testDispositions;

        public int NumberOfTestCasesToGenerate { get; }

        public TestCaseGenerator(IOracle oracle, KeyExpectationProvider validityTestCaseOptions)
        {
            _oracle = oracle;
            _testDispositions = validityTestCaseOptions;
            NumberOfTestCasesToGenerate = _testDispositions.ExpectationCount;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new SafePrimesKeyVerParameters()
            {
                SafePrime = group.SafePrimeGroup,
                DomainParameters = group.DomainParameters,
                Disposition = _testDispositions.GetRandomReason()
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
