using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.TLSv13.RFC8446
{
    public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 25;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup @group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 5;
            }
            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new TlsKdfv13Parameters
            {
                HashAlg = group.HmacAlg,
                RunningMode = group.RunningMode,
                RandomLength = group.RandomLength
            };

            try
            {
                var result = await _oracle.GetTlsv13CaseAsync(param);

                var testCase = new TestCase
                {
                    Dhe = result.Dhe,
                    Psk = result.Psk,

                    HelloClientRandom = result.HelloClientRandom,
                    HelloServerRandom = result.HelloServerRandom,

                    FinishedClientRandom = result.FinishClientRandom,
                    FinishedServerRandom = result.FinishServerRandom,

                    ClientEarlyTrafficSecret = result.DerivedKeyingMaterial.EarlySecretResult.ClientEarlyTrafficSecret,
                    EarlyExporterMasterSecret = result.DerivedKeyingMaterial.EarlySecretResult.EarlyExporterMasterSecret,

                    ClientHandshakeTrafficSecret = result.DerivedKeyingMaterial.HandshakeSecretResult.ClientHandshakeTrafficSecret,
                    ServerHandshakeTrafficSecret = result.DerivedKeyingMaterial.HandshakeSecretResult.ServerHandshakeTrafficSecret,

                    ClientApplicationTrafficSecret = result.DerivedKeyingMaterial.MasterSecretResult.ClientApplicationTrafficSecret,
                    ServerApplicationTrafficSecret = result.DerivedKeyingMaterial.MasterSecretResult.ServerApplicationTrafficSecret,
                    ExporterMasterSecret = result.DerivedKeyingMaterial.MasterSecretResult.ExporterMasterSecret,
                    ResumptionMasterSecret = result.DerivedKeyingMaterial.MasterSecretResult.ResumptionMasterSecret,
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
