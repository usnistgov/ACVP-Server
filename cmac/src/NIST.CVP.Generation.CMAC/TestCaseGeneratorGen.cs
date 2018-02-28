using System;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.Common.MAC;
using NIST.CVP.Crypto.Common.MAC.CMAC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.CMAC
{
    public class TestCaseGeneratorGen<TTestGroup, TTestCase> : ITestCaseGenerator<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase
        where TTestCase : TestCaseBase, new()
    {
        private readonly ICmac _algo;
        private readonly IRandom800_90 _random800_90;

        public int NumberOfTestCasesToGenerate => 8;

        public TestCaseGeneratorGen(IRandom800_90 random800_90, ICmac algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public TestCaseGenerateResponse Generate(TTestGroup @group, bool isSample)
        {
            //known answer - need to do an encryption operation to get the tag
            var key = _random800_90.GetRandomBitString(@group.KeyLength);
            var msg = _random800_90.GetRandomBitString(@group.MessageLength);
            var testCase = new TTestCase
            {
                Key = key,
                Message = msg
            };
            return Generate(@group, testCase);
        }

        public TestCaseGenerateResponse Generate(TTestGroup @group, TTestCase testCase)
        {
            MacResult genResult = null;
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
            testCase.Mac = genResult.Mac;

            return new TestCaseGenerateResponse(testCase);
        }
        
        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
