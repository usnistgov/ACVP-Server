using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Br2;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_SSC.Sp800_56Br2.Ifc
{
    public class TestCaseGeneratorVal : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        public int NumberOfTestCasesToGenerate { get; private set; }

        public TestCaseGeneratorVal(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            NumberOfTestCasesToGenerate = group.KasSscExpectationProvider.ExpectationCount;
            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup @group, bool isSample, int caseNo = -1)
        {
            try
            {
                var serverRequirements = KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                    group.Scheme, group.KasMode,
                    KeyGenerationRequirementsHelper.GetOtherPartyKeyAgreementRole(group.KasRole),
                    KeyConfirmationRole.None,
                    KeyConfirmationDirection.None);

                var iutRequirements = KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                    group.Scheme, group.KasMode,
                    group.KasRole,
                    KeyConfirmationRole.None,
                    KeyConfirmationDirection.None);

                KeyPair serverKey = serverRequirements.GeneratesEphemeralKeyPair ? group.ShuffleKeys.Pop() : null;
                KeyPair iutKey = iutRequirements.GeneratesEphemeralKeyPair ? group.ShuffleKeys.Pop() : null;

                var result = await _oracle.GetKasIfcSscValTestAsync(new KasSscValParametersIfc()
                {
                    Disposition = group.KasSscExpectationProvider.GetRandomReason(),
                    Modulo = group.Modulo,
                    Scheme = group.Scheme,
                    KasMode = group.KasMode,
                    PublicExponent = group.PublicExponent,
                    KeyGenerationMethod = group.KeyGenerationMethod,
                    IutKeyAgreementRole = group.KasRole,
                    HashFunctionZ = group.HashFunctionZ,

                    ServerGenerationRequirements = group.ServerRequirements,
                    IutGenerationRequirements = group.IutRequirements,

                    ServerKeyPair = serverKey,
                    IutKeyPair = iutKey,
                }, true);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase()
                {
                    Deferred = false,
                    TestPassed = result.TestPassed,
                    TestCaseDisposition = result.Disposition,
                    ServerZ = result.ServerZ,
                    ServerC = result.ServerC,
                    ServerKey = result.ServerKeyPair ?? new KeyPair() { PubKey = new PublicKey() },

                    IutZ = result.IutZ,
                    IutC = result.IutC,
                    IutKey = result.IutKeyPair ?? new KeyPair() { PubKey = new PublicKey() },

                    Z = result.Z,
                    HashZ = result.HashZ,
                    IutHashZ = result.IutHashZ
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
