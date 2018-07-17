using System;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CTR
{
    public class TestCaseGeneratorSingleBlock : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; } = 10;

        public TestCaseGeneratorSingleBlock(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            // This is a little hacky... but single block CTR is the same as OFB. So we can get past the awkward factory
            // TODO fix this up
            var param = new TdesParameters
            {
                DataLength = 64,
                KeyingOption = TdesHelpers.GetKeyingOptionFromNumberOfKeys(group.NumberOfKeys),
                Direction = group.Direction,
                Mode = BlockCipherModesOfOperation.Ofb
            };

            try
            {
                var result = _oracle.GetTdesCase(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Key = result.Key,
                    Iv = result.Iv,
                    PlainText = result.PlainText,
                    CipherText = result.CipherText
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
