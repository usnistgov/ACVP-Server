using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;
using NLog;
using System;

namespace NIST.CVP.Generation.AES_CTR
{
    public class TestCaseGeneratorCounter : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        private int _numberOfBlocks = 1000;

        public int NumberOfTestCasesToGenerate { get; } = 1;

        public TestCaseGeneratorCounter(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                _numberOfBlocks = 100;
            }

            var param = new CounterParameters<AesParameters>
            {
                Parameters = new AesParameters
                {
                    Mode = BlockCipherModesOfOperation.Ctr,
                    Direction = group.Direction,
                    DataLength = 128 * _numberOfBlocks,
                    KeyLength = group.KeyLength
                },
                Overflow = group.OverflowCounter,
                Incremental = group.IncrementalCounter
            };

            AesResult result = null;
            try
            {
                if (isSample)
                {
                    // Generate full test case
                    result = _oracle.CompleteDeferredAesCounterCase(param);
                }
                else
                {
                    // Generate partial test case
                    result = _oracle.GetDeferredAesCounterCase(param);
                }
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }

            return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
            {
                Deferred = true,
                CipherText = result.CipherText,
                IV = result.Iv,
                Key = result.Key,
                PlainText = result.PlainText,
                Length = result.PlainText?.BitLength ?? result.CipherText.BitLength
            });
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            return null;
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
