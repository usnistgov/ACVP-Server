using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.KDF;
using NIST.CVP.Crypto.KDF.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.KDF
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IKdf _algo;

        public int NumberOfTestCasesToGenerate { get; private set; } = 5;

        public TestCaseGenerator(IRandom800_90 rand, IKdf algo)
        {
            _random800_90 = rand;
            _algo = algo;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 2;
            }

            var kI = _random800_90.GetRandomBitString(128);
            var iv = _random800_90.GetRandomBitString(group.ZeroLengthIv ? 0 : 128);

            var testCase = new TestCase
            {
                KeyIn = kI,
                IV = iv,
                Deferred = true,
                
            };

            if (isSample)
            {
                // Generate fixed data (random data for now)
                testCase.FixedData = _random800_90.GetRandomBitString(128);

                // If we are doing this specific mode, we need a break location for the counter to occur
                if (group.KdfMode == KdfModes.Counter && group.CounterLocation == CounterLocations.MiddleFixedData)
                {
                    testCase.BreakLocation = _random800_90.GetRandomInt(1, 128);
                }

                return Generate(group, testCase);
            }
            else
            {
                return new TestCaseGenerateResponse(testCase);
            }
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            KdfResult kdfResult = null;
            try
            {
                kdfResult = _algo.DeriveKey(testCase.KeyIn, testCase.FixedData, group.KeyOutLength, testCase.IV,
                    testCase.BreakLocation);
                if (!kdfResult.Success)
                {
                    ThisLogger.Warn(kdfResult.ErrorMessage);
                    return new TestCaseGenerateResponse(kdfResult.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex.StackTrace);
                return new TestCaseGenerateResponse(ex.Message);
            }

            testCase.KeyOut = kdfResult.DerivedKey.GetDeepCopy();
            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
