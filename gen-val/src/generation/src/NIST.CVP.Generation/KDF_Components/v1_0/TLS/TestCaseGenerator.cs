using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.KDF_Components.v1_0.TLS
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
            var param = new TlsKdfParameters
            {
                HashAlg = group.HashAlg,
                PreMasterSecretLength = group.PreMasterSecretLength,
                TlsMode = group.TlsMode,
                KeyBlockLength = group.KeyBlockLength
            };

            try
            {
                var result = await _oracle.GetTlsKdfCaseAsync(param);

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
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        public ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
