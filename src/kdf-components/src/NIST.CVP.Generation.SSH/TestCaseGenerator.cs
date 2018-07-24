using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NLog;
using System;

namespace NIST.CVP.Generation.SSH
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 100;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
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

            SshKdfResult result = null;
            try
            {
                result = _oracle.GetSshKdfCase(param);
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }

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

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            return null;
        }

        public Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
