using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Crypto.KAS.Helpers;
using NIST.CVP.Crypto.KAS.Scheme.Ecc;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.ECC
{
    public class DeferredTestCaseResolverAftNoKdfNoKc : DeferredTestCaseResolverBaseEcc
    {
        public DeferredTestCaseResolverAftNoKdfNoKc(
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
            IMacParametersBuilder macParametersBuilder,
            IEntropyProviderFactory entropyProviderFactory
        ) : base(curveFactory, kasBuilder, schemeBuilder, macParametersBuilder, entropyProviderFactory) { }

        protected override IKas<
            KasDsaAlgoAttributesEcc, 
            OtherPartySharedInformation<
                EccDomainParameters, 
                EccKeyPair
            >, 
            EccDomainParameters, 
            EccKeyPair
        > GetServerKas(
            SchemeKeyNonceGenRequirement<EccScheme> serverKeyRequirements, 
            KeyAgreementRole serverRole,
            KeyConfirmationRole serverKcRole, 
            MacParameters macParameters, 
            TestGroup testGroup, 
            TestCase iutTestCase
        )
        {
            return _kasBuilder
                .WithKeyAgreementRole(serverKeyRequirements.ThisPartyKasRole)
                .WithPartyId(testGroup.IdServer)
                .WithKasDsaAlgoAttributes(testGroup.KasDsaAlgoAttributes)
                .WithSchemeBuilder(
                    _schemeBuilder
                        .WithHashFunction(testGroup.HashAlg)
                )
                .BuildNoKdfNoKc()
                .Build();
        }
    }
}