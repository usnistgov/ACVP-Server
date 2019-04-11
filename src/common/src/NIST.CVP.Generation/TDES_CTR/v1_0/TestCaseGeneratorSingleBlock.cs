using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.TDES_CTR.v1_0
{
    public class TestCaseGeneratorSingleBlock : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; } = 10;

        public TestCaseGeneratorSingleBlock(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            // This is a little hacky... but single block CTR is the same as OFB. So we can get past the awkward factory
            // TODO fix this up
            var param = new TdesParameters
            {
                DataLength = 64,
                KeyingOption = group.KeyingOption,
                Direction = group.Direction,
                Mode = BlockCipherModesOfOperation.Ofb
            };

            try
            {
                var result = await _oracle.GetTdesCaseAsync(param);

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
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
