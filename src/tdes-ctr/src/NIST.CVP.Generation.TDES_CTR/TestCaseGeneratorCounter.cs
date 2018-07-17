using System;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CTR
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

            var param = new CounterParameters<TdesParameters>
            {
                Parameters = new TdesParameters
                {
                    Mode = BlockCipherModesOfOperation.Ctr,
                    Direction = group.Direction,
                    DataLength = 64 * _numberOfBlocks,
                    KeyingOption = TdesHelpers.GetKeyingOptionFromNumberOfKeys(group.NumberOfKeys)
                },
                Overflow = group.OverflowCounter,
                Incremental = group.IncrementalCounter
            };
            try
            {

                TdesResult result = null;
                if (isSample)
                {
                    // Generate full test case
                    result = _oracle.CompleteDeferredTdesCounterCase(param);
                }
                else
                {
                    // Generate partial test case
                    result = _oracle.GetDeferredTdesCounterCase(param);
                }

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Deferred = true,
                    CipherText = result.CipherText,
                    Iv = result.Iv,
                    Key = result.Key,
                    PlainText = result.PlainText,
                    Length = result.PlainText?.BitLength ?? result.CipherText.BitLength
                });
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            throw new NotImplementedException();
        }
    }
}
