using System;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.NoKC;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Crypto.KAS.Scheme.Ecc;
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
            IKdfOneStepFactory kdfFactory,
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

            switch (schemeParameters.KasAlgoAttributes.Scheme)
            {
                case EccScheme.FullUnified:
                    scheme = new SchemeEccFullUnified(dsa, _eccCurveFactory, _withKdfFactory,
                        _withKeyConfirmationFactory, _withNoKeyConfirmationFactory, _withOtherInfoFactory,
                        _withEntropyProvider, schemeParameters, kdfParameters, macParameters, _withDiffieHellman);
                    break;
                case EccScheme.FullMqv:
                    scheme = new SchemeEccFullMqv(dsa, _eccCurveFactory, _withKdfFactory,
                        _withKeyConfirmationFactory, _withNoKeyConfirmationFactory, _withOtherInfoFactory,
                        _withEntropyProvider, schemeParameters, kdfParameters, macParameters, _withMqv);
                    break;
                case EccScheme.EphemeralUnified:
                    scheme = new SchemeEccEphemeralUnified(dsa, _eccCurveFactory, _withKdfFactory,
                        _withKeyConfirmationFactory, _withNoKeyConfirmationFactory, _withOtherInfoFactory,
                        _withEntropyProvider, schemeParameters, kdfParameters, macParameters, _withDiffieHellman);
                    break;
                case EccScheme.OnePassUnified:
                    scheme = new SchemeEccOnePassUnified(dsa, _eccCurveFactory, _withKdfFactory,
                        _withKeyConfirmationFactory, _withNoKeyConfirmationFactory, _withOtherInfoFactory,
                        _withEntropyProvider, schemeParameters, kdfParameters, macParameters, _withDiffieHellman);
                    break;
                case EccScheme.OnePassMqv:
                    scheme = new SchemeEccOnePassMqv(dsa, _eccCurveFactory, _withKdfFactory,
                        _withKeyConfirmationFactory, _withNoKeyConfirmationFactory, _withOtherInfoFactory,
                        _withEntropyProvider, schemeParameters, kdfParameters, macParameters, _withMqv);
                    break;
                case EccScheme.OnePassDh:
                    scheme = new SchemeEccOnePassDh(dsa, _eccCurveFactory, _withKdfFactory,
                        _withKeyConfirmationFactory, _withNoKeyConfirmationFactory, _withOtherInfoFactory,
                        _withEntropyProvider, schemeParameters, kdfParameters, macParameters, _withDiffieHellman);
                    break;
                case EccScheme.StaticUnified:
                    scheme = new SchemeEccStaticUnified(dsa, _eccCurveFactory, _withKdfFactory,
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