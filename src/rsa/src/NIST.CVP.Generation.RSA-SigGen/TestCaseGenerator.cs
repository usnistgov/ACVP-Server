using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NLog;
using System;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 10;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
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
                SaltLength = group.SaltLen
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
                    Signature = result.Signature,
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
