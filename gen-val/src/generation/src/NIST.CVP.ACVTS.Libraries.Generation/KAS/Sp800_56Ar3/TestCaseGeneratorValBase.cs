using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar3;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.Sp800_56Ar3
{
    public abstract class TestCaseGeneratorValBase<TTestGroup, TTestCase, TKeyPair> : ITestCaseGeneratorAsync<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKeyPair>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKeyPair>, new()
        where TKeyPair : IDsaKeyPair
    {
        private readonly IOracle _oracle;
        private readonly ITestCaseExpectationProvider<KasValTestDisposition> _testDispositions;

        protected TestCaseGeneratorValBase(IOracle oracle, ITestCaseExpectationProvider<KasValTestDisposition> validityTestCaseOptions)
        {
            _oracle = oracle;
            _testDispositions = validityTestCaseOptions;
            NumberOfTestCasesToGenerate = _testDispositions.ExpectationCount;
        }

        public int NumberOfTestCasesToGenerate { get; }
        public async Task<TestCaseGenerateResponse<TTestGroup, TTestCase>> GenerateAsync(TTestGroup @group, bool isSample, int caseNo = -1)
        {
            var testCaseDisposition = _testDispositions.GetRandomReason().GetReason();

            try
            {
                var result = await _oracle.GetKasValTestAsync(new KasValParameters()
                {
                    Disposition = testCaseDisposition,
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
