using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.ECC
{
    public class TestGroup : TestGroupBase<TestGroup, TestCase, KasDsaAlgoAttributesEcc>
    {
        public EccScheme Scheme { get; set; }
        public EccParameterSet ParmSet { get; set; }
        public Curve Curve { get; set; }

        public override KasDsaAlgoAttributesEcc KasAlgoAttributes =>
            new KasDsaAlgoAttributesEcc(Scheme, ParmSet, Curve);

        public override SchemeKeyNonceGenRequirement KeyNonceGenRequirementsIut =>
            KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                KasAlgoAttributes.Scheme, KasMode, KasRole, KcRole, KcType
            );

        public override SchemeKeyNonceGenRequirement KeyNonceGenRequirementsServer =>
            KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                KasAlgoAttributes.Scheme,
                KasMode,
                KeyGenerationRequirementsHelper.GetOtherPartyKeyAgreementRole(KasRole),
                KeyGenerationRequirementsHelper.GetOtherPartyKeyConfirmationRole(KcRole),
                KcType
            );
    }
}
