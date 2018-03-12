using System;
using NIST.CVP.Crypto.Common.MAC;
using NIST.CVP.Crypto.Common.MAC.CMAC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.CMAC
{
    public abstract class TestCaseGeneratorGenBase<TTestGroup, TTestCase> : ITestCaseGenerator<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase>, new()
    {
        protected readonly ICmac _algo;
        protected readonly IRandom800_90 _random800_90;

        public int NumberOfTestCasesToGenerate => 8;

        protected TestCaseGeneratorGenBase(IRandom800_90 random800_90, ICmac algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public abstract TestCaseGenerateResponse<TTestGroup, TTestCase> Generate(TTestGroup @group, bool isSample);


        public TestCaseGenerateResponse<TTestGroup, TTestCase> Generate(TTestGroup @group, TTestCase testCase)
        {
            MacResult genResult = null;
            try
            {
                genResult = _algo.Generate(testCase.Key, testCase.Message, group.MacLength);
                if (!genResult.Success)
                {
                    ThisLogger.Warn(genResult.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse<TTestGroup, TTestCase>(genResult.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                {
                    return new TestCaseGenerateResponse<TTestGroup, TTestCase>(ex.Message);
                }
            }
            testCase.Mac = genResult.Mac;

            return new TestCaseGenerateResponse<TTestGroup, TTestCase>(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
