using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Br2;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_IFC.Sp800_56Br2
{
    public class TestCaseGeneratorVal : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly ITestCaseExpectationProvider<KasIfcValTestDisposition> _testDispositions;

        public TestCaseGeneratorVal(IOracle oracle, ITestCaseExpectationProvider<KasIfcValTestDisposition> validityTestCaseOptions)
        {
            _oracle = oracle;
            _testDispositions = validityTestCaseOptions;
            NumberOfTestCasesToGenerate = _testDispositions.ExpectationCount;
        }

        public int NumberOfTestCasesToGenerate { get; }
        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup @group, bool isSample, int caseNo = -1)
        {
            var testCaseDisposition = _testDispositions.GetRandomReason().GetReason();

            try
            {
                var serverRequirements = KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                    group.Scheme, group.KasMode,
                    KeyGenerationRequirementsHelper.GetOtherPartyKeyAgreementRole(group.KasRole),
                    KeyGenerationRequirementsHelper.GetOtherPartyKeyConfirmationRole(group.KeyConfirmationRole),
                    group.KeyConfirmationDirection);

                var iutRequirements = KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                    group.Scheme, group.KasMode,
                    group.KasRole,
                    group.KeyConfirmationRole,
                    group.KeyConfirmationDirection);


                KeyPair serverKey = serverRequirements.GeneratesEphemeralKeyPair ? group.ShuffleKeys.Pop() : null;
                KeyPair iutKey = iutRequirements.GeneratesEphemeralKeyPair ? group.ShuffleKeys.Pop() : null;

                var result = await _oracle.GetKasValTestIfcAsync(new KasValParametersIfc()
                {
                    Disposition = testCaseDisposition,
                    L = group.L,
                    Modulo = group.Modulo,
                    Scheme = group.Scheme,
                    KasMode = group.KasMode,
                    KdfConfiguration = group.KdfConfiguration,
                    KtsConfiguration = group.KtsConfiguration,
                    MacConfiguration = group.MacConfiguration,
                    PublicExponent = group.PublicExponent,
                    IutPartyId = group.IutId,
                    ServerPartyId = group.ServerId,
                    KeyConfirmationDirection = group.KeyConfirmationDirection,
                    KeyGenerationMethod = group.KeyGenerationMethod,
                    IutKeyAgreementRole = group.KasRole,
                    IutKeyConfirmationRole = group.KeyConfirmationRole
                }, serverKey, iutKey);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase()
                {
                    Deferred = false,
                    TestPassed = result.TestPassed,
                    TestCaseDisposition = result.Disposition,
                    ServerZ = result.ServerZ,
                    ServerC = result.ServerC,
                    ServerNonce = result.ServerNonce,
                    ServerK = result.ServerK,
                    ServerKey = result.ServerKeyPair ?? new KeyPair() { PubKey = new PublicKey() },

                    IutZ = result.IutZ,
                    IutC = result.IutC,
                    IutNonce = result.IutNonce,
                    IutK = result.IutK,
                    IutKey = result.IutKeyPair ?? new KeyPair() { PubKey = new PublicKey() },

                    KdfParameter = result.KdfParameter,
                    KtsParameter = result.KtsParameter,

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
