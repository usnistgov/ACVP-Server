using System;
using NIST.CVP.Crypto.DSA;
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
    public abstract class SchemeBuilderBase<TKasDsaAlgoAttributes, TSharedInformation, TDomainParameters, TKeyPair> 
        : ISchemeBuilder<TKasDsaAlgoAttributes, TSharedInformation, TDomainParameters, TKeyPair>
        where TKasDsaAlgoAttributes : IKasDsaAlgoAttributes
        where TSharedInformation : ISharedInformation<TDomainParameters, TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        private readonly IKdfFactory _originalKdfFactory;
        private readonly IKeyConfirmationFactory _originalKeyConfirmationFactory;
        private readonly INoKeyConfirmationFactory _originalNoKeyConfirmationFactory;
        private readonly IOtherInfoFactory _originalOtherInfoFactory;
        private readonly IEntropyProvider _originalEntropyProvider;
        private readonly IDiffieHellman<TDomainParameters, TKeyPair> _originalDiffieHellmanFfc;
        private readonly IMqv<TDomainParameters, TKeyPair> _originalMqv;

        protected HashFunction _withHashFunction;
        protected IKdfFactory _withKdfFactory;
        protected IKeyConfirmationFactory _withKeyConfirmationFactory;
        protected INoKeyConfirmationFactory _withNoKeyConfirmationFactory;
        protected IOtherInfoFactory _withOtherInfoFactory;
        protected IEntropyProvider _withEntropyProvider;
        protected IDiffieHellman<TDomainParameters, TKeyPair> _withDiffieHellman;
        protected IMqv<TDomainParameters, TKeyPair> _withMqv;

        protected SchemeBuilderBase(
            IKdfFactory kdfFactory,
            IKeyConfirmationFactory keyConfirmationFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IOtherInfoFactory otherInfoFactory,
            IEntropyProvider entropyProvider,
            IDiffieHellman<TDomainParameters, TKeyPair> diffieHellmanFfc,
            IMqv<TDomainParameters, TKeyPair> mqv
        )
        {
            _originalKdfFactory = kdfFactory;
            _originalKeyConfirmationFactory = keyConfirmationFactory;
            _originalNoKeyConfirmationFactory = noKeyConfirmationFactory;
            _originalOtherInfoFactory = otherInfoFactory;
            _originalEntropyProvider = entropyProvider;
            _originalDiffieHellmanFfc = diffieHellmanFfc;
            _originalMqv = mqv;

            SetWithInjectablesToConstructionState();
        }

        public ISchemeBuilder<
            TKasDsaAlgoAttributes, 
            TSharedInformation, 
            TDomainParameters, 
            TKeyPair
        > 
            WithHashFunction(HashFunction hashFunction)
        {
            _withHashFunction = hashFunction;
            return this;
        }

        public ISchemeBuilder<
            TKasDsaAlgoAttributes,
            TSharedInformation,
            TDomainParameters,
            TKeyPair
        > 
            WithKdfFactory(IKdfFactory kdfFactory)
        {
            _withKdfFactory = kdfFactory;
            return this;
        }

        public ISchemeBuilder<
            TKasDsaAlgoAttributes,
            TSharedInformation,
            TDomainParameters,
            TKeyPair
        > 
            WithKeyConfirmationFactory(IKeyConfirmationFactory keyConfirmationFactory)
        {
            _withKeyConfirmationFactory = keyConfirmationFactory;
            return this;
        }

        public ISchemeBuilder<
            TKasDsaAlgoAttributes,
            TSharedInformation,
            TDomainParameters,
            TKeyPair
        > 
            WithNoKeyConfirmationFactory(INoKeyConfirmationFactory noKeyConfirmationFactory)
        {
            _withNoKeyConfirmationFactory = noKeyConfirmationFactory;
            return this;
        }

        public ISchemeBuilder<
            TKasDsaAlgoAttributes,
            TSharedInformation,
            TDomainParameters,
            TKeyPair
        > 
            WithOtherInfoFactory(IOtherInfoFactory otherInfoFactory)
        {
            _withOtherInfoFactory = otherInfoFactory;
            return this;
        }

        public ISchemeBuilder<
            TKasDsaAlgoAttributes,
            TSharedInformation,
            TDomainParameters,
            TKeyPair
        > WithEntropyProvider(IEntropyProvider entropyProvider)
        {
            _withEntropyProvider = entropyProvider;
            return this;
        }

        public abstract IScheme<
            SchemeParametersBase<TKasDsaAlgoAttributes>,
            TKasDsaAlgoAttributes, 
            TSharedInformation, 
            TDomainParameters, 
            TKeyPair
        > 
            BuildScheme(
                SchemeParametersBase<TKasDsaAlgoAttributes> schemeParameters, 
                KdfParameters kdfParameters, 
                MacParameters macParameters,
                bool backToOriginalState = true
            );
        
        protected void SetWithInjectablesToConstructionState()
        {
            _withKdfFactory = _originalKdfFactory;
            _withKeyConfirmationFactory = _originalKeyConfirmationFactory;
            _withNoKeyConfirmationFactory = _originalNoKeyConfirmationFactory;
            _withOtherInfoFactory = _originalOtherInfoFactory;
            _withEntropyProvider = _originalEntropyProvider;
            _withDiffieHellman = _originalDiffieHellmanFfc;
            _withMqv = _originalMqv;
        }
    }
}