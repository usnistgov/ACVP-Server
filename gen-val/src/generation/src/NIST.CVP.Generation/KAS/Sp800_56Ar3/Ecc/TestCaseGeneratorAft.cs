using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.KAS.Sp800_56Ar3.Ecc
{
    public class TestCaseGeneratorAft : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestCaseGeneratorAft(IOracle oracle)
        {
            _oracle = oracle;
        }

        public int NumberOfTestCasesToGenerate => 10;
        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup @group, bool isSample, int caseNo = -1)
        {
            try
            {
                var result = await _oracle.GetKasAftTestAsync(new KasAftParameters()
                {
                    DomainParameters = group.DomainParameters,
                    KasScheme = group.Scheme,
                    IutGenerationRequirements = group.KeyNonceGenRequirementsIut,
                    ServerGenerationRequirements = group.KeyNonceGenRequirementsServer
                });

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase()
                {
                    Deferred = true,
                    TestPassed = true,
                    EphemeralKeyServer = (EccKeyPair) result.ServerSecretKeyingMaterial.EphemeralKeyPair,
                    StaticKeyServer = (EccKeyPair) result.ServerSecretKeyingMaterial.StaticKeyPair,
                    DkmNonceServer = result.ServerSecretKeyingMaterial.DkmNonce,
                    EphemeralNonceServer = result.ServerSecretKeyingMaterial.EphemeralNonce
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
            }
        }
        
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    }
}