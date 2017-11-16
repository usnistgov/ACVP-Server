using System;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.KAS.Builders
{
    public class SchemeBuilderFfc 
        : SchemeBuilderBase<
            FfcParameterSet, 
            FfcScheme, 
            FfcSharedInformation<
                FfcDomainParameters, 
                FfcKeyPair
            >, 
            FfcDomainParameters, 
            FfcKeyPair
        >
    {
        public SchemeBuilderFfc(
            IDsaFfcFactory dsaFactory, 
            IKdfFactory kdfFactory, 
            IKeyConfirmationFactory keyConfirmationFactory, 
            INoKeyConfirmationFactory noKeyConfirmationFactory, 
            IOtherInfoFactory<FfcSharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> otherInfoFactory, 
            IEntropyProvider entropyProvider, 
            IDiffieHellman<FfcDomainParameters, FfcKeyPair> diffieHellmanFfc, 
            IMqv<FfcDomainParameters, FfcKeyPair> mqv) 
            : base(
                  dsaFactory, 
                  kdfFactory, 
                  keyConfirmationFactory, 
                  noKeyConfirmationFactory, 
                  otherInfoFactory, 
                  entropyProvider, 
                  diffieHellmanFfc, 
                  mqv)
        {
        }

        public override IScheme<
            SchemeParametersBase<
                FfcParameterSet, 
                FfcScheme
            >, 
            FfcParameterSet, 
            FfcScheme,
            FfcSharedInformation<
                FfcDomainParameters, 
                FfcKeyPair
            >, 
            FfcDomainParameters, 
            FfcKeyPair
        > 
            BuildScheme(
            SchemeParametersBase<FfcParameterSet, FfcScheme> schemeParameters, 
            KdfParameters kdfParameters, 
            MacParameters macParameters, 
            bool backToOriginalState = true
        )
        {
            IScheme<
                SchemeParametersBase<
                    FfcParameterSet,
                    FfcScheme
                >,
                FfcParameterSet,
                FfcScheme,
                FfcSharedInformation<
                    FfcDomainParameters,
                    FfcKeyPair
                >,
                FfcDomainParameters,
                FfcKeyPair
            > scheme = null;

            var dsa = _withDsaFactory.GetInstance(_withHashFunction);

            switch (schemeParameters.Scheme)
            {
                case FfcScheme.DhEphem:
                    scheme = new SchemeFfcDiffieHellmanEphemeral(dsa, _withKdfFactory,
                        _withKeyConfirmationFactory, _withNoKeyConfirmationFactory, _withOtherInfoFactory,
                        _withEntropyProvider, schemeParameters, kdfParameters, macParameters, _withDiffieHellmanFfc);
                    break;
                case FfcScheme.Mqv1:
                    scheme = new SchemeFfcMqv1(dsa, _withKdfFactory,
                        _withKeyConfirmationFactory, _withNoKeyConfirmationFactory, _withOtherInfoFactory,
                        _withEntropyProvider, schemeParameters, kdfParameters, macParameters, _withMqv);
                    break;
                case FfcScheme.DhHybrid1:
                case FfcScheme.DhHybridOneFlow:
                case FfcScheme.DhOneFlow:
                case FfcScheme.DhStatic:

                case FfcScheme.Mqv2:
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