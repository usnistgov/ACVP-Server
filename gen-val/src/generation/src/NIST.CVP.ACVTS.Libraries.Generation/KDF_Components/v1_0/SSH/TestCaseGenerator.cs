using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SSH
{
    public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 100;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup @group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 20;
            }
            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
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
