﻿using System;
using NIST.CVP.Crypto.Common.MAC;
using NIST.CVP.Crypto.Common.MAC.CMAC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.CMAC
{
    public class TestCaseGeneratorVer<TTestGroup, TTestCase> : ITestCaseGenerator<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase>, new()
    {
        private readonly ICmac _algo;
        private readonly IRandom800_90 _random800_90;

        public int NumberOfTestCasesToGenerate => 20;

        public TestCaseGeneratorVer(IRandom800_90 random800_90, ICmac algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public TestCaseGenerateResponse<TTestGroup, TTestCase> Generate(TTestGroup @group, bool isSample)
        {
            var key = _random800_90.GetRandomBitString(@group.KeyLength);
            var msg = _random800_90.GetRandomBitString(@group.MessageLength);
            var testCase = new TTestCase
            {
                Key = key,
                Message = msg,
                TestPassed = true
            };
            return Generate(@group, testCase);
        }

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
            
            SometimesMangleTestCaseMac(testCase);

            return new TestCaseGenerateResponse<TTestGroup, TTestCase>(testCase);
        }

        private void SometimesMangleTestCaseMac(TTestCase testCase)
        {
            // Alter the tag 25% of the time for a "failure" test
            int option = _random800_90.GetRandomInt(0, 4);
            if (option == 0)
            {
                testCase.Mac = _random800_90.GetDifferentBitStringOfSameSize(testCase.Mac);
                testCase.TestPassed = false;
            }
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}