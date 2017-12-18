using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.DSA.ECC.Helpers;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Helpers;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.KAS.Scheme.Ecc;
using NIST.CVP.Generation.KAS.ECC.Helpers;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.ECC
{
    public abstract class TestCaseGeneratorAftBaseEcc : TestCaseGeneratorAftBase<
        TestGroup,
        TestCase,
        KasDsaAlgoAttributesEcc,
        EccDomainParameters,
        EccKeyPair,
        EccScheme
    >
    {

        protected readonly IEccCurveFactory _curveFactory;

        protected TestCaseGeneratorAftBaseEcc(
            IEccCurveFactory curveFactory,
            IKasBuilder<
                KasDsaAlgoAttributesEcc, 
                OtherPartySharedInformation<
                    EccDomainParameters, 
                    EccKeyPair
                >, 
                EccDomainParameters, 
                EccKeyPair
            > kasBuilder, 
            ISchemeBuilder<
                KasDsaAlgoAttributesEcc, 
                OtherPartySharedInformation<
                    EccDomainParameters, 
                    EccKeyPair
                >, 
                EccDomainParameters, 
                EccKeyPair
            > schemeBuilder, 
            IEntropyProviderFactory entropyProviderFactory, 
            IMacParametersBuilder macParametersBuilder
        ) : base(kasBuilder, schemeBuilder, entropyProviderFactory, macParametersBuilder)
        {
            _curveFactory = curveFactory;
        }

        /// <inheritdoc />
        protected override SchemeKeyNonceGenRequirement<EccScheme> GetPartyNonceKeyGenRequirements(
            TestGroup testGroup,
            KeyAgreementRole partyKeyAgreementRole, 
            KeyConfirmationRole partyKeyConfirmationRole
        )
        {
            return KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                testGroup.Scheme,
                testGroup.KasMode,
                partyKeyAgreementRole,
                partyKeyConfirmationRole,
                testGroup.KcType
            );
        }

        /// <inheritdoc />
        protected override int GetEphemeralLengthRequirement(TestGroup testGroup)
        {
            var curveAttributes = CurveAttributesHelper.GetCurveAttribute(testGroup.CurveName);
            return curveAttributes.LengthN * 2;
        }

        /// <inheritdoc />
        protected override int GetDkmLengthRequirement(TestGroup testGroup)
        {
            var curveAttributes = CurveAttributesHelper.GetCurveAttribute(testGroup.CurveName);
            return curveAttributes.LengthN / 2;
        }

        /// <inheritdoc />
        protected override EccDomainParameters GetGroupDomainParameters(TestGroup testGroup)
        {
            return new EccDomainParameters(_curveFactory.GetCurve(testGroup.CurveName));
        }

        /// <inheritdoc />
        protected override void SetTestCaseInformationFromKasResult(
            TestGroup @group, 
            TestCase testCase, 
            IKas<
                KasDsaAlgoAttributesEcc, 
                OtherPartySharedInformation<
                    EccDomainParameters, 
                    EccKeyPair
                >, 
                EccDomainParameters, 
                EccKeyPair
            > serverKas, 
            IKas<
                KasDsaAlgoAttributesEcc, 
                OtherPartySharedInformation<
                    EccDomainParameters, 
                    EccKeyPair
                >, 
                EccDomainParameters, 
                EccKeyPair
            > iutKas,
            KasResult iutResult)
        {
            TestCaseDispositionHelper.SetTestCaseInformationFromKasResults(
                group,
                testCase,
                serverKas,
                iutKas,
                iutResult
            );
        }
    }
}