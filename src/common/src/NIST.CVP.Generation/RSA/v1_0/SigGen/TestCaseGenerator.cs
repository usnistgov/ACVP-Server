using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;
using System;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA.v1_0.SigGen
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
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 3;
            }

            var param = new RsaSignatureParameters
            {
                HashAlg = group.HashAlg,
                Key = group.Key,
                Modulo = group.Modulo,
                PaddingScheme = group.Mode,
                SaltLength = group.SaltLen,
                IsMessageRandomized = group.IsMessageRandomized
            };

            try
            {
                RsaSignatureResult result = null;
                if (isSample)
                {
                    result = await _oracle.GetRsaSignatureAsync(param);
                }
                else
                {
                    result = await _oracle.GetDeferredRsaSignatureAsync(param);
                }

                var testCase = new TestCase
                {
                    Message = result.Message,
                    RandomValue = result.RandomValue,
                    RandomValueLen = result.RandomValue?.BitLength ?? 0,
                    Signature = result.Signature?.PadToModulusMsb(@group.Modulo),
                    Salt = result.Salt
                };

                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        private ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
