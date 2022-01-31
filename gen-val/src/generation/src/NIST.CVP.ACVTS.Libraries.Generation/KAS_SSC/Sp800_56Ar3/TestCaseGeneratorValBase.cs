using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar3;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_SSC.Sp800_56Ar3
{
    public abstract class TestCaseGeneratorValBase<TTestGroup, TTestCase, TKeyPair> : ITestCaseGeneratorAsync<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKeyPair>, new()
        where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKeyPair>, new()
        where TKeyPair : IDsaKeyPair
    {
        private readonly IOracle _oracle;
        private readonly ITestCaseExpectationProvider<KasSscTestCaseExpectation> _testCaseExpectationProvider;

        public TestCaseGeneratorValBase(IOracle oracle, ITestCaseExpectationProvider<KasSscTestCaseExpectation> testCaseExpectationProvider, int numberOfTestCasesToGenerate)
        {
            _oracle = oracle;
            _testCaseExpectationProvider = testCaseExpectationProvider;
            NumberOfTestCasesToGenerate = numberOfTestCasesToGenerate;
        }

        public int NumberOfTestCasesToGenerate { get; }
        public async Task<TestCaseGenerateResponse<TTestGroup, TTestCase>> GenerateAsync(TTestGroup @group, bool isSample, int caseNo = -1)
        {
            var disposition = _testCaseExpectationProvider.GetRandomReason();

            try
            {
                var result = await _oracle.GetKasSscValTestAsync(new KasSscValParameters()
                {
                    Disposition = disposition.GetReason(),
                    DomainParameters = group.DomainParameters,
                    KasDpGeneration = group.DomainParameterGenerationMode,
                    KasAlgorithm = group.KasAlgorithm,
                    KasScheme = group.Scheme,
                    HashFunctionZ = group.HashFunctionZ,
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
                    Z = result.SharedSecretComputationResult.Z,
                    HashZ = result.SharedSecretComputationResult.HashZ,
                    TestPassed = result.TestPassed,
                    EphemeralKeyIut = GetKey(result.IutSecretKeyingMaterial.EphemeralKeyPair),
                    StaticKeyIut = GetKey(result.IutSecretKeyingMaterial.StaticKeyPair),
                    EphemeralKeyServer = GetKey(result.ServerSecretKeyingMaterial.EphemeralKeyPair),
                    StaticKeyServer = GetKey(result.ServerSecretKeyingMaterial.StaticKeyPair),
                    TestCaseDisposition = result.Disposition,
                });
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return new TestCaseGenerateResponse<TTestGroup, TTestCase>(e.Message);
            }
        }

        protected abstract TKeyPair GetKey(IDsaKeyPair keyPair);

        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    }
}
