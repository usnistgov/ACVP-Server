using System;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.CMAC
{
    public abstract class TestCaseGeneratorGenBase<TTestGroup, TTestCase> : ITestCaseGenerator<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestCase>
        where TTestCase : TestCaseBase, new()
    {
        protected readonly ICmac _algo;
        protected readonly IRandom800_90 _random800_90;

        public int NumberOfTestCasesToGenerate => 8;

        public TestCaseGeneratorGenBase(IRandom800_90 random800_90, ICmac algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public abstract TestCaseGenerateResponse Generate(TTestGroup @group, bool isSample);


        public TestCaseGenerateResponse Generate(TTestGroup @group, TTestCase testCase)
        {
            CmacResult genResult = null;
            try
            {
                genResult = _algo.Generate(testCase.Key, testCase.Message, group.MacLength);
                if (!genResult.Success)
                {
                    ThisLogger.Warn(genResult.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse(genResult.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                {
                    return new TestCaseGenerateResponse(ex.Message);
                }
            }
            testCase.Mac = genResult.ResultingMac;

            return new TestCaseGenerateResponse(testCase);
        }
        
        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
