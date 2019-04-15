using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.SHA2.v1_0
{
    public class TestCaseGeneratorMCTHash : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public bool IsSample { get; set; } = false;

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGeneratorMCTHash(IOracle oracle)
        {
            _oracle = oracle;
        }
        
        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            IsSample = isSample;
            var param = new ShaParameters
            {
                HashFunction = new HashFunction(group.Function, group.DigestSize),
                MessageLength = SHAEnumHelpers.DigestSizeToInt(group.DigestSize)
            };

            try
            {
                var oracleResult = await _oracle.GetShaMctCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Message = oracleResult.Seed.Message,
                    ResultsArray = oracleResult.Results.ConvertAll(element => new AlgoArrayResponse { Message = element.Message, Digest = element.Digest })
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }
        
        private ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

