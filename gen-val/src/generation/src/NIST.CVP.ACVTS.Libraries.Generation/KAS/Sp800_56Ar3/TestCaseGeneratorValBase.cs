using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar3;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.Sp800_56Ar3
{
    public abstract class TestCaseGeneratorValBase<TTestGroup, TTestCase, TKeyPair> : ITestCaseGeneratorWithPrep<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKeyPair>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKeyPair>, new()
        where TKeyPair : IDsaKeyPair
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; }

        protected TestCaseGeneratorValBase(IOracle oracle)
        {
            _oracle = oracle;
        }
        
        public GenerateResponse PrepareGenerator(TTestGroup group, bool isSample)
        {
            NumberOfTestCasesToGenerate = group.KasExpectationProvider.ExpectationCount;
            return new GenerateResponse();
        }
        
        public async Task<TestCaseGenerateResponse<TTestGroup, TTestCase>> GenerateAsync(TTestGroup @group, bool isSample, int caseNo = -1)
        {
            try
            {
                var result = await _oracle.GetKasValTestAsync(new KasValParameters()
                {
                    Disposition = group.KasExpectationProvider.GetRandomReason(),
                    L = group.L,
                    KasScheme = group.Scheme,
                    KasAlgorithm = group.KasAlgorithm,
                    DomainParameters = group.DomainParameters,
                    KasDpGeneration = group.DomainParameterGenerationMode,
                    KdfConfiguration = group.KdfConfiguration,
                    MacConfiguration = group.MacConfiguration,
                    PartyIdIut = group.IutId,
                    PartyIdServer = group.ServerId,
                    IutGenerationRequirements = group.KeyNonceGenRequirementsIut,
                    ServerGenerationRequirements = group.KeyNonceGenRequirementsServer,

                    ServerEphemeralKey = group.KeyNonceGenRequirementsServer.GeneratesEphemeralKeyPair ? group.ShuffleKeys.Pop() : default,
                    ServerStaticKey = group.KeyNonceGenRequirementsServer.GeneratesStaticKeyPair ? group.ShuffleKeys.Pop() : default,

                    IutEphemeralKey = group.KeyNonceGenRequirementsIut.GeneratesEphemeralKeyPair ? group.ShuffleKeys.Pop() : default,
                    IutStaticKey = group.KeyNonceGenRequirementsIut.GeneratesStaticKeyPair ? group.ShuffleKeys.Pop() : default,
                }, true);

                return new TestCaseGenerateResponse<TTestGroup, TTestCase>(new TTestCase()
                {
                    Deferred = false,
                    TestPassed = result.TestPassed,
                    TestCaseDisposition = result.Disposition,

                    EphemeralKeyServer = GetKey(result.ServerSecretKeyingMaterial.EphemeralKeyPair),
                    StaticKeyServer = GetKey(result.ServerSecretKeyingMaterial.StaticKeyPair),
                    EphemeralNonceServer = result.ServerSecretKeyingMaterial.EphemeralNonce,
                    DkmNonceServer = result.ServerSecretKeyingMaterial.DkmNonce,

                    EphemeralKeyIut = GetKey(result.IutSecretKeyingMaterial.EphemeralKeyPair),
                    StaticKeyIut = GetKey(result.IutSecretKeyingMaterial.StaticKeyPair),
                    EphemeralNonceIut = result.IutSecretKeyingMaterial.EphemeralNonce,
                    DkmNonceIut = result.IutSecretKeyingMaterial.DkmNonce,

                    KdfParameter = result.KdfParameter,

                    Z = result.KasResult.Z,

                    MacKey = result.KasResult.MacKey,
                    MacData = result.KasResult.MacData,

                    Dkm = result.KasResult.Dkm,
                    Tag = result.KasResult.Tag
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new TestCaseGenerateResponse<TTestGroup, TTestCase>(ex.Message);
            }
        }

        protected abstract TKeyPair GetKey(IDsaKeyPair keyPair);

        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    }
}
