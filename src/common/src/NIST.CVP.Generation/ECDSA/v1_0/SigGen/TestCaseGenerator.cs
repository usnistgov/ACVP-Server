using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;
using System;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.ECDSA.v1_0.SigGen
{
    public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 10;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new EcdsaSignatureParameters
            {
                Curve = group.Curve,
                HashAlg = group.HashAlg,
                PreHashedMessage = group.ComponentTest,
                Key = group.KeyPair,
                IsMessageRandomized = group.IsMessageRandomized
            };

            try
            {
                TestCase testCase = null;
                EcdsaSignatureResult result = null;
                if (isSample)
                {
                    result = await _oracle.GetEcdsaSignatureAsync(param);
                    testCase = new TestCase
                    {
                        Message = result.Message,
                        RandomValue = result.RandomValue,
                        RandomValueLen = result.RandomValue?.BitLength ?? 0,
                        Signature = result.Signature
                    };

                }
                else
                {
                    result = await _oracle.GetDeferredEcdsaSignatureAsync(param);
                    testCase = new TestCase
                    {
                        Message = result.Message
                    };
                }

                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating case: {ex.Message}");
            }
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
