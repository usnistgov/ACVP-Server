using System;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.CMAC
{
    public abstract class TestCaseGeneratorVerBase<TTestGroup, TTestCase> : ITestCaseGenerator<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase>, new()
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => 20;

        protected TestCaseGeneratorVerBase(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TTestGroup, TTestCase> Generate(TTestGroup @group, bool isSample)
        {
            var param = GetParam(group);

            try
            {
                var oracleResult = _oracle.GetCmacCase(param);

                return new TestCaseGenerateResponse<TTestGroup, TTestCase>(new TTestCase
                {
                    Key = oracleResult.Key,
                    Message = oracleResult.Message,
                    Mac = oracleResult.Tag,
                    TestPassed = oracleResult.TestPassed
                });
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TTestGroup, TTestCase>($"Failed to generate. {ex.Message}");
            }
        }

        public TestCaseGenerateResponse<TTestGroup, TTestCase> Generate(TTestGroup @group, TTestCase testCase)
        {
            throw new NotImplementedException();
        }

        protected abstract CmacParameters GetParam(TTestGroup group);
    }
}