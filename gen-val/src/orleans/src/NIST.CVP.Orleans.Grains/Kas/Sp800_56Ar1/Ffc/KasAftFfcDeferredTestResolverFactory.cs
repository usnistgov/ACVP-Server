using System;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Orleans.Grains.Kas.Sp800_56Ar1.Ffc
{
    public class KasAftFfcDeferredTestResolverFactory : IKasAftDeferredTestResolverFactory<KasAftDeferredParametersFfc, KasAftDeferredResult>
    {
        private readonly IKasBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>,
            FfcDomainParameters, FfcKeyPair
        > _kasBuilder;
        private readonly ISchemeBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>,
            FfcDomainParameters, FfcKeyPair
        > _schemeBuilder;
        private readonly IMacParametersBuilder _macParametersBuilder;
        private readonly IEntropyProviderFactory _entropyProviderFactory;

        public KasAftFfcDeferredTestResolverFactory(
            IKasBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair
            > kasBuilder,
            ISchemeBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair
            > schemeBuilder,
            IMacParametersBuilder macParametersBuilder,
            IEntropyProviderFactory entropyProviderFactory
        )
        {
            _kasBuilder = kasBuilder;
            _schemeBuilder = schemeBuilder;
            _macParametersBuilder = macParametersBuilder;
            _entropyProviderFactory = entropyProviderFactory;
        }

        public IKasAftDeferredTestResolver<KasAftDeferredParametersFfc, KasAftDeferredResult> GetInstance(KasMode kasMode)
        {
            switch (kasMode)
            {
                case KasMode.NoKdfNoKc:
                    return new KasAftFfcDeferredTestResolverNoKdfNoKc(_kasBuilder, _schemeBuilder, _macParametersBuilder, _entropyProviderFactory);
                case KasMode.KdfNoKc:
                    return new KasAftFfcDeferredTestResolverKdfNoKc(_kasBuilder, _schemeBuilder, _macParametersBuilder, _entropyProviderFactory);
                case KasMode.KdfKc:
                    return new KasAftFfcDeferredTestResolverKdfKc(_kasBuilder, _schemeBuilder, _macParametersBuilder, _entropyProviderFactory);
                default:
                    throw new ArgumentException(nameof(kasMode));
            }
        }
    }
}