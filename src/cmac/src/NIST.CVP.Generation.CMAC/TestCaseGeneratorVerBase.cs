using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.CMAC
{
    public abstract class TestCaseGeneratorVerBase : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => 20;

        protected TestCaseGeneratorVerBase(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup @group, bool isSample)
        {
            var param = GetParam(group);

            try
            {
                var oracleResult = await _oracle.GetCmacCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Key = oracleResult.Key,
                    Message = oracleResult.Message,
                    Mac = oracleResult.Tag,
                    TestPassed = oracleResult.TestPassed
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        protected abstract CmacParameters GetParam(TestGroup group);
        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}