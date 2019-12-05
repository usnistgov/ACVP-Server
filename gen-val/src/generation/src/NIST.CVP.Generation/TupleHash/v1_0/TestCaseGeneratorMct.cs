using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Hash.TupleHash;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.TupleHash.v1_0
{
    public class TestCaseGeneratorMct : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGeneratorMct(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new TupleHashParameters
            {
                HashFunction = new HashFunction(group.DigestSize, group.DigestSize * 2, group.XOF),
                InputLengths = group.MessageLength.GetDeepCopy(),
                OutputLengths = group.OutputLength.GetDeepCopy(),
                IsSample = isSample
            };

            try
            {
                var oracleResult = await _oracle.GetTupleHashMctCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Tuple = oracleResult.Seed.Tuple,
                    ResultsArray = oracleResult.Results.ConvertAll(element => new AlgoArrayResponse { Tuple = element.Tuple, Digest = element.Digest, Customization = element.Customization })
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
