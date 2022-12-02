using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.NoKC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Scheme.Ffc;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Builders.Ffc
{
    public class SchemeBuilderFfc
        : SchemeBuilderBase<
            KasDsaAlgoAttributesFfc,
            OtherPartySharedInformation<
                FfcDomainParameters,
                FfcKeyPair
            >,
            FfcDomainParameters,
            FfcKeyPair
        >
    {
        private readonly IDsaFfcFactory _dsaFfcFactory;

        public SchemeBuilderFfc(
            IDsaFfcFactory dsaFfcFactory,
            IKdfOneStepFactory kdfFactory,
            IKeyConfirmationFactory keyConfirmationFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IOtherInfoFactory otherInfoFactory,
            IEntropyProvider entropyProvider,
            IDiffieHellman<FfcDomainParameters, FfcKeyPair> diffieHellmanFfc,
            IMqv<FfcDomainParameters, FfcKeyPair> mqv)
            : base(
                  kdfFactory,
                  keyConfirmationFactory,
                  noKeyConfirmationFactory,
                  otherInfoFactory,
                  entropyProvider,
                  diffieHellmanFfc,
                  mqv)
        {
            _dsaFfcFactory = dsaFfcFactory;
        }

        public override IScheme<
            SchemeParametersBase<KasDsaAlgoAttributesFfc>,
            KasDsaAlgoAttributesFfc,
            OtherPartySharedInformation<
                FfcDomainParameters,
                FfcKeyPair
            >,
            FfcDomainParameters,
            FfcKeyPair
        >
            BuildScheme(
            SchemeParametersBase<KasDsaAlgoAttributesFfc> schemeParameters,
            KdfParameters kdfParameters,
            MacParameters macParameters,
            bool backToOriginalState = true
        )
        {
            IScheme<
                SchemeParametersBase<KasDsaAlgoAttributesFfc>,
                KasDsaAlgoAttributesFfc,
                OtherPartySharedInformation<
                    FfcDomainParameters,
                    FfcKeyPair
                >,
                FfcDomainParameters,
                FfcKeyPair
            > scheme = null;

            var dsa = _dsaFfcFactory.GetInstance(_withHashFunction);

            switch (schemeParameters.KasAlgoAttributes.Scheme)
            {
                case FfcScheme.DhHybrid1:
                    scheme = new SchemeFfcDhHybrid1(dsa, _withKdfFactory,
                        _withKeyConfirmationFactory, _withNoKeyConfirmationFactory, _withOtherInfoFactory,
                        _withEntropyProvider, schemeParameters, kdfParameters, macParameters, _withDiffieHellman);
                    break;
                case FfcScheme.Mqv2:
                    scheme = new SchemeFfcMqv2(dsa, _withKdfFactory,
                        _withKeyConfirmationFactory, _withNoKeyConfirmationFactory, _withOtherInfoFactory,
                        _withEntropyProvider, schemeParameters, kdfParameters, macParameters, _withMqv);
                    break;
                case FfcScheme.DhEphem:
                    scheme = new SchemeFfcDiffieHellmanEphemeral(dsa, _withKdfFactory,
                        _withKeyConfirmationFactory, _withNoKeyConfirmationFactory, _withOtherInfoFactory,
                        _withEntropyProvider, schemeParameters, kdfParameters, macParameters, _withDiffieHellman);
                    break;
                case FfcScheme.DhHybridOneFlow:
                    scheme = new SchemeFfcDhHybridOneFlow(dsa, _withKdfFactory,
                        _withKeyConfirmationFactory, _withNoKeyConfirmationFactory, _withOtherInfoFactory,
                        _withEntropyProvider, schemeParameters, kdfParameters, macParameters, _withDiffieHellman);
                    break;
                case FfcScheme.Mqv1:
                    scheme = new SchemeFfcMqv1(dsa, _withKdfFactory,
                        _withKeyConfirmationFactory, _withNoKeyConfirmationFactory, _withOtherInfoFactory,
                        _withEntropyProvider, schemeParameters, kdfParameters, macParameters, _withMqv);
                    break;
                case FfcScheme.DhOneFlow:
                    scheme = new SchemeFfcDhOneFlow(dsa, _withKdfFactory,
                        _withKeyConfirmationFactory, _withNoKeyConfirmationFactory, _withOtherInfoFactory,
                        _withEntropyProvider, schemeParameters, kdfParameters, macParameters, _withDiffieHellman);
                    break;
                case FfcScheme.DhStatic:
                    scheme = new SchemeFfcDhStatic(dsa, _withKdfFactory,
                        _withKeyConfirmationFactory, _withNoKeyConfirmationFactory, _withOtherInfoFactory,
                        _withEntropyProvider, schemeParameters, kdfParameters, macParameters, _withDiffieHellman);
                    break;
                default:
                    throw new ArgumentException(nameof(schemeParameters.KasAlgoAttributes.Scheme));
            }

            if (backToOriginalState)
            {
                SetWithInjectablesToConstructionState();
            }

            return scheme;
        }
    }
}
