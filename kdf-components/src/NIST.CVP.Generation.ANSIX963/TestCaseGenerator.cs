using System;
using NIST.CVP.Crypto.Common.KDF.Components.AnsiX963;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.ANSIX963
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly IAnsiX963 _algo;

        public int NumberOfTestCasesToGenerate { get; private set; } = 100;

        public TestCaseGenerator(IRandom800_90 rand, IAnsiX963 algo)
        {
            _rand = rand;
            _algo = algo;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 20;
            }

            var testCase = new TestCase
            {
                Z = _rand.GetRandomBitString(group.FieldSize),
                SharedInfo = _rand.GetRandomBitString(group.SharedInfoLength)
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            KdfResult kdfResult = null;
            try
            {
                kdfResult = _algo.DeriveKey(testCase.Z, testCase.SharedInfo, group.KeyDataLength);
                if (!kdfResult.Success)
                {
                    ThisLogger.Warn(kdfResult.ErrorMessage);
                    return new TestCaseGenerateResponse<TestGroup, TestCase>(kdfResult.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex.StackTrace);
                return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
            }

            testCase.KeyData = kdfResult.DerivedKey;
            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
