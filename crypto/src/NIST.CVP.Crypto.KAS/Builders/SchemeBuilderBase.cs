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
    public abstract class SchemeBuilderBase<TParameterSet, TScheme> : ISchemeBuilder<TParameterSet, TScheme>
        where TParameterSet : struct, IComparable
        where TScheme : struct, IComparable
    {
        private readonly IDsaFfcFactory _originalDsaFactory;
        private readonly IKdfFactory _originalKdfFactory;
        private readonly IKeyConfirmationFactory _originalKeyConfirmationFactory;
        private readonly INoKeyConfirmationFactory _originalNoKeyConfirmationFactory;
        private readonly IOtherInfoFactory _originalOtherInfoFactory;
        private readonly IEntropyProvider _originalEntropyProvider;
        private readonly IDiffieHellman<FfcDomainParameters, FfcKeyPair> _originalDiffieHellmanFfc;
        private readonly IMqv<FfcDomainParameters, FfcKeyPair> _originalMqv;

        protected HashFunction _withHashFunction;
        protected IDsaFfcFactory _withDsaFactory;
        protected IKdfFactory _withKdfFactory;
        protected IKeyConfirmationFactory _withKeyConfirmationFactory;
        protected INoKeyConfirmationFactory _withNoKeyConfirmationFactory;
        protected IOtherInfoFactory _withOtherInfoFactory;
        protected IEntropyProvider _withEntropyProvider;
        protected IDiffieHellman<FfcDomainParameters, FfcKeyPair> _withDiffieHellmanFfc;
        protected IMqv<FfcDomainParameters, FfcKeyPair> _withMqv;

        protected SchemeBuilderBase(
            IDsaFfcFactory dsaFactory,
            IKdfFactory kdfFactory,
            IKeyConfirmationFactory keyConfirmationFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IOtherInfoFactory otherInfoFactory,
            IEntropyProvider entropyProvider,
            IDiffieHellman<FfcDomainParameters, FfcKeyPair> diffieHellmanFfc,
            IMqv<FfcDomainParameters, FfcKeyPair> mqv
        )
        {
            _originalDsaFactory = dsaFactory;
            _originalKdfFactory = kdfFactory;
            _originalKeyConfirmationFactory = keyConfirmationFactory;
            _originalNoKeyConfirmationFactory = noKeyConfirmationFactory;
            _originalOtherInfoFactory = otherInfoFactory;
            _originalEntropyProvider = entropyProvider;
            _originalDiffieHellmanFfc = diffieHellmanFfc;
            _originalMqv = mqv;

            SetWithInjectablesToConstructionState();
        }

        public ISchemeBuilder<TParameterSet, TScheme> WithHashFunction(HashFunction hashFunction)
        {
            _withHashFunction = hashFunction;
            return this;
        }

        public ISchemeBuilder<TParameterSet, TScheme> WithDsaFactory(IDsaFfcFactory dsaFactory)
        {
            _withDsaFactory = dsaFactory;
            return this;
        }

        public ISchemeBuilder<TParameterSet, TScheme> WithKdfFactory(IKdfFactory kdfFactory)
        {
            _withKdfFactory = kdfFactory;
            return this;
        }

        public ISchemeBuilder<TParameterSet, TScheme> WithKeyConfirmationFactory(IKeyConfirmationFactory keyConfirmationFactory)
        {
            _withKeyConfirmationFactory = keyConfirmationFactory;
            return this;
        }

        public ISchemeBuilder<TParameterSet, TScheme> WithNoKeyConfirmationFactory(INoKeyConfirmationFactory noKeyConfirmationFactory)
        {
            _withNoKeyConfirmationFactory = noKeyConfirmationFactory;
            return this;
        }

        public ISchemeBuilder<TParameterSet, TScheme> WithOtherInfoFactory(IOtherInfoFactory otherInfoFactory)
        {
            _withOtherInfoFactory = otherInfoFactory;
            return this;
        }

        public ISchemeBuilder<TParameterSet, TScheme> WithEntropyProvider(IEntropyProvider entropyProvider)
        {
            _withEntropyProvider = entropyProvider;
            return this;
        }

        public abstract IScheme<SchemeParametersBase<TParameterSet, TScheme>, TParameterSet, TScheme> BuildScheme(
            SchemeParametersBase<TParameterSet, TScheme> schemeParameters, 
            KdfParameters kdfParameters, 
            MacParameters macParameters,
            bool backToOriginalState = true
        );
        
        protected void SetWithInjectablesToConstructionState()
        {
            _withDsaFactory = _originalDsaFactory;
            _withKdfFactory = _originalKdfFactory;
            _withKeyConfirmationFactory = _originalKeyConfirmationFactory;
            _withNoKeyConfirmationFactory = _originalNoKeyConfirmationFactory;
            _withOtherInfoFactory = _originalOtherInfoFactory;
            _withEntropyProvider = _originalEntropyProvider;
            _withDiffieHellmanFfc = _originalDiffieHellmanFfc;
            _withMqv = _originalMqv;
        }
    }
}