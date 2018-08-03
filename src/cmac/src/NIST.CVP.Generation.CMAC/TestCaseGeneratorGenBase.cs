using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.CMAC
{
    public abstract class TestCaseGeneratorGenBase<TTestGroup, TTestCase> : ITestCaseGeneratorAsync<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase>, new()
    {
        protected readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => 8;

        protected TestCaseGeneratorGenBase(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TTestGroup, TTestCase>> GenerateAsync(TTestGroup group, bool isSample)
        {
            var param = GetParam(group);

            try
            {
                var oracleResult = await _oracle.GetCmacCaseAsync(param);
                
                return new TestCaseGenerateResponse<TTestGroup, TTestCase>(new TTestCase
                {
                    Key = oracleResult.Key,
                    Message = oracleResult.Message,
                    Mac = oracleResult.Tag
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TTestGroup, TTestCase>($"Failed to generate. {ex.Message}");
            }
        }

        protected abstract CmacParameters GetParam(TTestGroup group);
        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
