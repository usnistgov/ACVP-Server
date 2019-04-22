using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.CMAC.v1_0
{
    public abstract class TestCaseGeneratorGenBase : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        protected readonly IOracle Oracle;

        public int NumberOfTestCasesToGenerate => 8;

        protected TestCaseGeneratorGenBase(IOracle oracle)
        {
            Oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            var param = GetParam(group);

            try
            {
                var oracleResult = await Oracle.GetCmacCaseAsync(param);
                
                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Key = oracleResult.Key,
                    Message = oracleResult.Message,
                    Mac = oracleResult.Tag
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
