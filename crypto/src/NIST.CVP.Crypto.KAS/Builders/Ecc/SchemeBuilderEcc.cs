using System;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.KAS.Scheme.Ecc;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.KAS.Builders.Ecc
{
    public class SchemeBuilderEcc
        : SchemeBuilderBase<
            KasDsaAlgoAttributesEcc,
            OtherPartySharedInformation<
                EccDomainParameters,
                EccKeyPair
            >,
            EccDomainParameters,
            EccKeyPair
        >
    {
        private readonly IDsaEccFactory _dsaEccFactory;
        private readonly IEccCurveFactory _eccCurveFactory;

        public SchemeBuilderEcc(
            IDsaEccFactory dsaEccFactory,
            IEccCurveFactory eccCurveFactory,
            IKdfFactory kdfFactory,
            IKeyConfirmationFactory keyConfirmationFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IOtherInfoFactory otherInfoFactory,
            IEntropyProvider entropyProvider,
            IDiffieHellman<EccDomainParameters, EccKeyPair> diffieHellmanEcc,
            IMqv<EccDomainParameters, EccKeyPair> mqv)
            : base(
                  kdfFactory,
                  keyConfirmationFactory,
                  noKeyConfirmationFactory,
                  otherInfoFactory,
                  entropyProvider,
                  diffieHellmanEcc,
                  mqv
              )
        {
            _dsaEccFactory = dsaEccFactory;
            _eccCurveFactory = eccCurveFactory;
        }

        public override IScheme<
            SchemeParametersBase<KasDsaAlgoAttributesEcc>,
            KasDsaAlgoAttributesEcc,
            OtherPartySharedInformation<
                EccDomainParameters,
                EccKeyPair
            >,
            EccDomainParameters,
            EccKeyPair
        >
            BuildScheme(
            SchemeParametersBase<KasDsaAlgoAttributesEcc> schemeParameters,
            KdfParameters kdfParameters,
            MacParameters macParameters,
            bool backToOriginalState = true
        )
        {
            IScheme<
                SchemeParametersBase<KasDsaAlgoAttributesEcc>,
                KasDsaAlgoAttributesEcc,
                OtherPartySharedInformation<
                    EccDomainParameters,
                    EccKeyPair
                >,
                EccDomainParameters,
                EccKeyPair
            > scheme = null;

            var dsa = _dsaEccFactory.GetInstance(_withHashFunction);

            switch (schemeParameters.KasDsaAlgoAttributes.Scheme)
            {
                case EccScheme.EphemeralUnified:
                    scheme = new SchemeEccEphemeralUnified(dsa, _eccCurveFactory, _withKdfFactory,
                        _withKeyConfirmationFactory, _withNoKeyConfirmationFactory, _withOtherInfoFactory,
                        _withEntropyProvider, schemeParameters, kdfParameters, macParameters, _withDiffieHellman);
                    break;
                case EccScheme.OnePassMqv:
                    scheme = new SchemeEccOnePassMqv(dsa, _eccCurveFactory, _withKdfFactory,
                        _withKeyConfirmationFactory, _withNoKeyConfirmationFactory, _withOtherInfoFactory,
                        _withEntropyProvider, schemeParameters, kdfParameters, macParameters, _withMqv);
                    break;
                case EccScheme.StaticUnified:
                    scheme = new SchemeEccStaticUnified(dsa, _eccCurveFactory, _withKdfFactory,
                        _withKeyConfirmationFactory, _withNoKeyConfirmationFactory, _withOtherInfoFactory,
                        _withEntropyProvider, schemeParameters, kdfParameters, macParameters, _withDiffieHellman);
                    break;
                case EccScheme.FullUnified:
                case EccScheme.OnePassDh:
                case EccScheme.OnePassUnified:
                case EccScheme.FullMqv:
                    // TODO coming soon to a KAS near you!
                    throw new NotImplementedException();
                default:
                    throw new ArgumentException(nameof(schemeParameters.KasDsaAlgoAttributes.Scheme));
            }

            if (backToOriginalState)
            {
                SetWithInjectablesToConstructionState();
            }

            return scheme;
        }
    }
}