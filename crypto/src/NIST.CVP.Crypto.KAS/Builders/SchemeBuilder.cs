using System;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.KAS.Builders
{
    public class SchemeBuilder : ISchemeBuilder
    {
        private readonly IShaFactory _shaFactory;
        private readonly IDsaFfcFactory _originalDsaFactory;
        private readonly IKdfFactory _originalKdfFactory;
        private readonly IKeyConfirmationFactory _originalKeyConfirmationFactory;
        private readonly INoKeyConfirmationFactory _originalNoKeyConfirmationFactory;
        private readonly IOtherInfoFactory _originalOtherInfoFactory;
        private readonly IEntropyProvider _originalEntropyProvider;
        private readonly IDiffieHellman _originalDiffieHellman;
        private readonly IMqv _originalMqv;

        private HashFunction _withHashFunction;
        private IDsaFfcFactory _withDsaFactory;
        private IKdfFactory _withKdfFactory;
        private IKeyConfirmationFactory _withKeyConfirmationFactory;
        private INoKeyConfirmationFactory _withNoKeyConfirmationFactory;
        private IOtherInfoFactory _withOtherInfoFactory;
        private IEntropyProvider _withEntropyProvider;
        private IDiffieHellman _withDiffieHellman;
        private IMqv _withMqv;
        
        public SchemeBuilder(
            IShaFactory shaFactory,
            IDsaFfcFactory dsaFactory,
            IKdfFactory kdfFactory,
            IKeyConfirmationFactory keyConfirmationFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IOtherInfoFactory otherInfoFactory,
            IEntropyProvider entropyProvider,
            IDiffieHellman diffieHellman,
            IMqv mqv
        )
        {
            _shaFactory = shaFactory;
            _originalDsaFactory = dsaFactory;
            _originalKdfFactory = kdfFactory;
            _originalKeyConfirmationFactory = keyConfirmationFactory;
            _originalNoKeyConfirmationFactory = noKeyConfirmationFactory;
            _originalOtherInfoFactory = otherInfoFactory;
            _originalEntropyProvider = entropyProvider;
            _originalDiffieHellman = diffieHellman;
            _originalMqv = mqv;

            SetWithInjectablesToConstructionState();
        }

        private void SetWithInjectablesToConstructionState()
        {
            _withDsaFactory = _originalDsaFactory;
            _withKdfFactory = _originalKdfFactory;
            _withKeyConfirmationFactory = _originalKeyConfirmationFactory;
            _withNoKeyConfirmationFactory = _originalNoKeyConfirmationFactory;
            _withOtherInfoFactory = _originalOtherInfoFactory;
            _withEntropyProvider = _originalEntropyProvider;
            _withDiffieHellman = _originalDiffieHellman;
            _withMqv = _originalMqv;
        }

        public ISchemeBuilder WithHashFunction(HashFunction hashFunction)
        {
            _withHashFunction = hashFunction;
            return this;
        }

        public ISchemeBuilder WithDsaFactory(IDsaFfcFactory dsaFactory)
        {
            _withDsaFactory = dsaFactory;
            return this;
        }

        public ISchemeBuilder WithKdfFactory(IKdfFactory kdfFactory)
        {
            _withKdfFactory = kdfFactory;
            return this;
        }

        public ISchemeBuilder WithKeyConfirmationFactory(IKeyConfirmationFactory keyConfirmationFactory)
        {
            _withKeyConfirmationFactory = keyConfirmationFactory;
            return this;
        }

        public ISchemeBuilder WithNoKeyConfirmationFactory(INoKeyConfirmationFactory noKeyConfirmationFactory)
        {
            _withNoKeyConfirmationFactory = noKeyConfirmationFactory;
            return this;
        }

        public ISchemeBuilder WithOtherInfoFactory(IOtherInfoFactory otherInfoFactory)
        {
            _withOtherInfoFactory = otherInfoFactory;
            return this;
        }

        public ISchemeBuilder WithEntropyProvider(IEntropyProvider entropyProvider)
        {
            _withEntropyProvider = entropyProvider;
            return this;
        }

        public ISchemeBuilder WithDiffieHellman(IDiffieHellman diffieHellman)
        {
            _withDiffieHellman = diffieHellman;
            return this;
        }

        public ISchemeBuilder WithMqv(IMqv mqv)
        {
            _withMqv = mqv;
            return this;
        }

        public IScheme BuildScheme(SchemeParameters schemeParameters, KdfParameters kdfParameters, MacParameters macParameters,
            bool backToOriginalState = true)
        {
            IScheme scheme = null;

            var dsa = _withDsaFactory.GetInstance(_withHashFunction);

            switch (schemeParameters.Scheme)
            {
                case FfcScheme.DhEphem:
                    scheme = new SchemeDiffieHellmanEphemeral(dsa, _withKdfFactory,
                        _withKeyConfirmationFactory, _withNoKeyConfirmationFactory, _withOtherInfoFactory,
                        _withEntropyProvider, schemeParameters, kdfParameters, macParameters, _withDiffieHellman);
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