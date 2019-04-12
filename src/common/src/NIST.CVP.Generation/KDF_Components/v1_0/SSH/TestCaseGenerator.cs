using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.KDF_Components.v1_0.SSH
{
    public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 100;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 20;
            }

            var param = new SshKdfParameters
            {
                Cipher = group.Cipher,
                HashAlg = group.HashAlg
            };

            try
            {
                var result = await _oracle.GetSshKdfCaseAsync(param);

                var testCase = new TestCase
                {
                    EncryptionKeyClient = result.EncryptionKeyClient,
                    K = result.K,
                    SessionId = result.SessionId,
                    H = result.H,
                    IntegrityKeyServer = result.IntegrityKeyServer,
                    IntegrityKeyClient = result.IntegrityKeyClient,
                    InitialIvServer = result.InitialIvServer,
                    EncryptionKeyServer = result.EncryptionKeyServer,
                    InitialIvClient = result.InitialIvClient
                };

                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        public ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
