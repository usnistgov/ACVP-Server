using System;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.KAS.Builders.Ecc
{
    public class SchemeBuilderEcc
        : SchemeBuilderBase<
            EccParameterSet,
            EccScheme,
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
            IOtherInfoFactory<OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> otherInfoFactory,
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
            SchemeParametersBase<
                EccParameterSet,
                EccScheme
            >,
            EccParameterSet,
            EccScheme,
            OtherPartySharedInformation<
                EccDomainParameters,
                EccKeyPair
            >,
            EccDomainParameters,
            EccKeyPair
        >
            BuildScheme(
            SchemeParametersBase<EccParameterSet, EccScheme> schemeParameters,
            KdfParameters kdfParameters,
            MacParameters macParameters,
            bool backToOriginalState = true
        )
        {
            IScheme<
                SchemeParametersBase<
                    EccParameterSet,
                    EccScheme
                >,
                EccParameterSet,
                EccScheme,
                OtherPartySharedInformation<
                    EccDomainParameters,
                    EccKeyPair
                >,
                EccDomainParameters,
                EccKeyPair
            > scheme = null;

            var dsa = _dsaEccFactory.GetInstance(_withHashFunction);

            switch (schemeParameters.Scheme)
            {
                case EccScheme.EphemeralUnified:
                    scheme = new SchemeEccEphemeralUnified(dsa, _eccCurveFactory, _withKdfFactory,
                        _withKeyConfirmationFactory, _withNoKeyConfirmationFactory, _withOtherInfoFactory,
                        _withEntropyProvider, schemeParameters, kdfParameters, macParameters, _withDiffieHellman);
                    break;
                case EccScheme.FullUnified:
                case EccScheme.OnePassDh:
                case EccScheme.OnePassUnified:
                case EccScheme.StaticUnified:

                case EccScheme.FullMqv:
                case EccScheme.OnePassMqv:
                    // TODO coming soon to a KAS near you!
                    throw new NotImplementedException();
                default:
                    throw new ArgumentException(nameof(schemeParameters.Scheme));
            }

            if (backToOriginalState)
            {
                SetWithInjectablesToConstructionState();
            }

            return scheme;
        }
    }
}