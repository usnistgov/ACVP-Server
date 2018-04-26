using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.ECC
{
    public abstract class DeferredTestCaseResolverBaseEcc : DeferredTestCaseResolverBase<
        TestGroup,
        TestCase,
        KasDsaAlgoAttributesEcc,
        EccDomainParameters,
        EccKeyPair,
        EccScheme
    >
    {
        protected readonly IEccCurveFactory _curveFactory;

        protected DeferredTestCaseResolverBaseEcc(
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
        ) : base(kasBuilder, schemeBuilder, macParametersBuilder, entropyProviderFactory)
        {
            _curveFactory = curveFactory;
        }

        /// <inheritdoc />
        protected override SchemeKeyNonceGenRequirement<EccScheme> GetServerNonceKeyGenRequirements(
            TestGroup testGroup,
            KeyAgreementRole serverKeyAgreementRole, 
            KeyConfirmationRole serverKeyConfirmationRole
        )
        {
            return KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                testGroup.Scheme,
                testGroup.KasMode,
                serverKeyAgreementRole,
                serverKeyConfirmationRole,
                testGroup.KcType
            );
        }

        /// <inheritdoc />
        protected override EccDomainParameters GetDomainParameters(TestGroup testGroup)
        {
            return new EccDomainParameters(_curveFactory.GetCurve(testGroup.CurveName));
        }

        /// <inheritdoc />
        protected override OtherPartySharedInformation<EccDomainParameters, EccKeyPair> 
            GetIutSharedInformation(
                TestGroup testGroup, 
                TestCase serverTestCase, 
                TestCase iutTestCase,
                EccDomainParameters domainParameters
            )
        {
            return new OtherPartySharedInformation<EccDomainParameters, EccKeyPair>(
                domainParameters,
                iutTestCase.IdIut ?? testGroup.IdIut,
                new EccKeyPair(new EccPoint(iutTestCase.StaticPublicKeyIutX, iutTestCase.StaticPublicKeyIutY)),
                new EccKeyPair(new EccPoint(iutTestCase.EphemeralPublicKeyIutX, iutTestCase.EphemeralPublicKeyIutY)),
                iutTestCase.DkmNonceIut,
                iutTestCase.EphemeralNonceIut,
                serverTestCase.NonceNoKc
            );
        }

        /// <inheritdoc />
        protected override void UpdateKasInstanceWithTestCaseInformation(
            IKas<
                KasDsaAlgoAttributesEcc, 
                OtherPartySharedInformation<
                    EccDomainParameters, 
                    EccKeyPair
                >, 
                EccDomainParameters, 
                EccKeyPair
            > serverKas, 
            SchemeKeyNonceGenRequirement<EccScheme> serverKeyRequirements,
            TestCase serverTestCase)
        {
            if (serverKeyRequirements.GeneratesStaticKeyPair)
            {
                serverKas.Scheme.StaticKeyPair.PrivateD = serverTestCase.StaticPrivateKeyServer;
                serverKas.Scheme.StaticKeyPair.PublicQ = new EccPoint(
                    serverTestCase.StaticPublicKeyServerX,
                    serverTestCase.StaticPublicKeyServerY
                );
            }

            if (serverKeyRequirements.GeneratesEphemeralKeyPair)
            {
                serverKas.Scheme.EphemeralKeyPair.PrivateD = serverTestCase.EphemeralPrivateKeyServer;
                serverKas.Scheme.EphemeralKeyPair.PublicQ = new EccPoint(
                    serverTestCase.EphemeralPublicKeyServerX,
                    serverTestCase.EphemeralPublicKeyServerY
                );
            }
        }
    }
}