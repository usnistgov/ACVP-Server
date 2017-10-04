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
    public class SchemeBuilder : ISchemeBuilder
    {
        private readonly IDsaFfc _originalDsa;
        private readonly IKdfFactory _originalKdfFactory;
        private readonly IKeyConfirmationFactory _originalKeyConfirmationFactory;
        private readonly INoKeyConfirmationFactory _originalNoKeyConfirmationFactory;
        private readonly IOtherInfoFactory _originalOtherInfoFactory;
        private readonly IEntropyProvider _originalEntropyProvider;

        private IDsaFfc _withDsa;
        private IOtherInfoFactory _withOtherInfoFactory;
        private IEntropyProvider _withEntropyProvider;

        public SchemeBuilder(
            IDsaFfc dsa,
            IKdfFactory kdfFactory,
            IKeyConfirmationFactory keyConfirmationFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IOtherInfoFactory otherInfoFactory,
            IEntropyProvider entropyProvider
        )
        {
            _originalDsa = dsa;
            _originalKdfFactory = kdfFactory;
            _originalKeyConfirmationFactory = keyConfirmationFactory;
            _originalNoKeyConfirmationFactory = noKeyConfirmationFactory;
            _originalOtherInfoFactory = otherInfoFactory;
            _originalEntropyProvider = entropyProvider;

            SetWithInjectablesToConstructionState();
        }

        private void SetWithInjectablesToConstructionState()
        {
            _withDsa = _originalDsa;
            _withOtherInfoFactory = _originalOtherInfoFactory;
            _withEntropyProvider = _originalEntropyProvider;
        }

        public ISchemeBuilder WithDsa(IDsaFfc dsa)
        {
            _withDsa = dsa;
            return this;
        }

        public ISchemeBuilder WithOtherInfoFactory(IOtherInfoFactory otherInfoFactory)
        {
            _withOtherInfoFactory = otherInfoFactory;
            return this;
        }

        public ISchemeBuilder WIthEntropyProvider(IEntropyProvider entropyProvider)
        {
            _withEntropyProvider = entropyProvider;
            return this;
        }

        public IScheme BuildScheme(SchemeParameters schemeParameters, KdfParameters kdfParameters, MacParameters macParameters,
            bool backToOriginalState = true)
        {
            IScheme scheme = null;

            switch (schemeParameters.Scheme)
            {
                case FfcScheme.DhEphem:
                    scheme = new SchemeDiffieHellmanEphemeral(_withDsa, _originalKdfFactory,
                        _originalKeyConfirmationFactory, _originalNoKeyConfirmationFactory, _withOtherInfoFactory,
                        _withEntropyProvider, schemeParameters, kdfParameters, macParameters, new DiffieHellman());
                    break;
                case FfcScheme.DhHybrid1:
                case FfcScheme.DhHybridOneFlow:
                case FfcScheme.DhOneFlow:
                case FfcScheme.DhStatic:
                case FfcScheme.Mqv1:
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