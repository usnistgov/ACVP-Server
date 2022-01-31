using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Br2;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_IFC.Sp800_56Br2
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

                var result = await _oracle.GetKasAftTestIfcAsync(new KasAftParametersIfc()
                {
                    IsSample = isSample,
                    L = group.L,
                    Modulo = group.Modulo,
                    PublicExponent = group.PublicExponent,
                    Scheme = group.Scheme,
                    KasMode = group.KasMode,
                    KdfConfiguration = group.KdfConfiguration,
                    KtsConfiguration = group.KtsConfiguration,
                    MacConfiguration = group.MacConfiguration,
                    KeyGenerationMethod = group.KeyGenerationMethod,
                    IutKeyAgreementRole = group.KasRole,
                    KeyConfirmationDirection = group.KeyConfirmationDirection,
                    IutKeyConfirmationRole = group.KeyConfirmationRole,
                    IutPartyId = group.IutId,
                    ServerPartyId = group.ServerId,
                }, serverKey, iutKey);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase()
                {
                    Deferred = true,
                    TestPassed = true,
                    ServerC = result.ServerC,
                    ServerK = result.ServerK,
                    ServerZ = result.ServerZ,
                    ServerNonce = result.ServerNonce,
                    ServerKey = result.ServerKeyPair ?? new KeyPair() { PubKey = new PublicKey() },
                    IutKey = result.IutKeyPair ?? new KeyPair() { PubKey = new PublicKey() },
                    KdfParameter = result.KdfParameter,
                    KtsParameter = result.KtsParameter
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
