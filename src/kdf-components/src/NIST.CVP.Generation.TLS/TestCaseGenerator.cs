using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NLog;
using System;

namespace NIST.CVP.Generation.TLS
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

            var param = new TlsKdfParameters
            {
                HashAlg = group.HashAlg,
                PreMasterSecretLength = group.PreMasterSecretLength,
                TlsMode = group.TlsMode,
                KeyBlockLength = group.KeyBlockLength
            };

            TlsKdfResult result = null;
            try
            {
                result = _oracle.GetTlsKdfCase(param);
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }

            var testCase = new TestCase
            {
                PreMasterSecret = result.PreMasterSecret,
                ClientHelloRandom = result.ClientHelloRandom,
                MasterSecret = result.MasterSecret,
                ServerHelloRandom = result.ServerHelloRandom,
                ServerRandom = result.ServerRandom,
                KeyBlock = result.KeyBlock,
                ClientRandom = result.ClientRandom
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
