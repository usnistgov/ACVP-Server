using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.KAS.Sp800_56Ar3.Helpers;
using NLog;

namespace NIST.CVP.Generation.KAS.Sp800_56Ar3.Ecc
{
    public class TestCaseGeneratorVal : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly List<KasValTestDisposition> _testDispositions;

        public TestCaseGeneratorVal(IOracle oracle, List<KasValTestDisposition> validityTestCaseOptions)
        {
            _oracle = oracle;
            _testDispositions = validityTestCaseOptions;
            NumberOfTestCasesToGenerate = _testDispositions.Count;
        }

        public int NumberOfTestCasesToGenerate { get; }
        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup @group, bool isSample, int caseNo = -1)
        {
            var testCaseDisposition = TestCaseDispositionHelper.GetTestCaseIntention(_testDispositions);
            
            try
            {
                var result = await _oracle.GetKasValTestAsync(new KasValParameters()
                {
                    Disposition = testCaseDisposition,
                    L = group.L,
                    KasScheme = group.Scheme,
                    DomainParameters = group.DomainParameters,
                    KdfConfiguration = group.KdfConfiguration,
                    MacConfiguration = group.MacConfiguration,
                    IutPartyId = group.IutId,
                    ServerPartyId = group.ServerId,
                    IutGenerationRequirements = group.KeyNonceGenRequirementsIut,
                    ServerGenerationRequirements = group.KeyNonceGenRequirementsServer
                });

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase()
                {
                    Deferred = false,
                    TestPassed = result.TestPassed,
                    TestCaseDisposition = result.Disposition,
                    
                    EphemeralKeyServer = (EccKeyPair) result.ServerSecretKeyingMaterial.EphemeralKeyPair,
                    StaticKeyServer = (EccKeyPair) result.ServerSecretKeyingMaterial.StaticKeyPair,
                    EphemeralNonceServer = result.ServerSecretKeyingMaterial.EphemeralNonce,
                    DkmNonceServer = result.ServerSecretKeyingMaterial.DkmNonce,
                    
                    EphemeralKeyIut = (EccKeyPair) result.IutSecretKeyingMaterial.EphemeralKeyPair,
                    StaticKeyIut = (EccKeyPair) result.IutSecretKeyingMaterial.StaticKeyPair,
                    EphemeralNonceIut = result.IutSecretKeyingMaterial.EphemeralNonce,
                    DkmNonceIut = result.IutSecretKeyingMaterial.DkmNonce,
                    
                    KdfParameter = result.KdfParameter,
                    
                    MacKey = result.KasResult.MacKey,
                    MacData = result.KasResult.MacData,
                    
                    Dkm = result.KasResult.Dkm,
                    Tag = result.KasResult.Tag
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