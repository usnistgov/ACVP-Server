using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Br2;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Br2;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces.Kas;

namespace NIST.CVP.Orleans.Grains.Kas
{
    public class OracleObserverKasAftIfcCaseGrain : ObservableOracleGrainBase<KasAftResultIfc>, IOracleObserverKasAftIfcCaseGrain
    {
        private readonly IKasIfcBuilder _kasBuilder;
        private readonly ISchemeIfcBuilder _schemeBuilder;
        private readonly IIfcSecretKeyingMaterialBuilder _serverSecretKeyingMaterialBuilder;
        private readonly IIfcSecretKeyingMaterialBuilder _iutSecretKeyingMaterialBuilder;
        private readonly IKdfParameterVisitor _kdfParameterVisitor;
        private readonly IEntropyProvider _entropyProvider;
        private readonly IRsaSve _rsaSve;
        
        private KasAftParametersIfc _param;
        private KeyPair _serverKeyPair;
        private KeyPair _iutKeyPair;

        public OracleObserverKasAftIfcCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IKasIfcBuilder kasBuilder,
            ISchemeIfcBuilder schemeBuilder,
            IIfcSecretKeyingMaterialBuilder serverSecretKeyingMaterialBuilder,
            IIfcSecretKeyingMaterialBuilder iutSecretKeyingMaterialBuilder,
            IKdfParameterVisitor kdfParameterVisitor,
            IEntropyProvider entropyProvider,
            IRsaSve rsaSve) 
            : base(nonOrleansScheduler)
        {
            _kasBuilder = kasBuilder;
            _schemeBuilder = schemeBuilder;
            _serverSecretKeyingMaterialBuilder = serverSecretKeyingMaterialBuilder;
            _iutSecretKeyingMaterialBuilder = iutSecretKeyingMaterialBuilder;
            _kdfParameterVisitor = kdfParameterVisitor;
            _entropyProvider = entropyProvider;
            _rsaSve = rsaSve;
        }

        public async Task<bool> BeginWorkAsync(KasAftParametersIfc param, KeyPair serverKeyPair, KeyPair iutKeyPair)
        {
            _param = param;
            _serverKeyPair = serverKeyPair;
            _iutKeyPair = iutKeyPair;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            _serverSecretKeyingMaterialBuilder
                .WithPartyId(_param.ServerPartyId)
                .WithKey(_serverKeyPair);

            var iutSecretKeyingMaterial = _iutSecretKeyingMaterialBuilder
                .WithPartyId(_param.IutPartyId)
                .WithKey(_iutKeyPair)
                .Build(
                    _param.Scheme, 
                    _param.KasMode, 
                    _param.IutKeyAgreementRole, 
                    _param.IutKeyConfirmationRole,
                    _param.KeyConfirmationDirection);
            
            // Create the server contributions
            /*
             * Party U
             * Kas1-basic, Kas2-basic, kas1-KC, kas2-KC
             * generate random value z of nlen bytes, encrypted with IUT public key
             * generate salt for kdf, if kdf uses a mac
             *
             * KTS-basic, KTS-KC
             * wrap random key of L bits, using IUT public key
             *
             * Party V
             * Kas1-basic, Kas2-basic, kas1-KC, kas2-KC
             * provide public key
             * generate salt for kdf, if kdf uses a mac
             *
             * kas1-basic, kas1-kc
             * provide nonce
             *
             * KTS-basic, KTS-KC
             * provide public key
             */
            _schemeBuilder
                .WithSchemeParameters(
                    new SchemeParametersIfc(
                        new KasAlgoAttributesIfc(_param.Scheme, _param.Modulo, _param.L), 
                        _param.ServerKeyAgreementRole, 
                        _param.KasMode, 
                        _param.ServerKeyConfirmationRole, 
                        _param.KeyConfirmationDirection, 
                        KasAssurance.None, 
                        _param.ServerPartyId))
                .WithThisPartyKeyingMaterialBuilder(_serverSecretKeyingMaterialBuilder)
                .WithRsaSve(_rsaSve)
                .WithEntropyProvider(_entropyProvider);
                
            var serverKas = _kasBuilder.WithSchemeBuilder(_schemeBuilder).Build();
            serverKas.InitializeThisPartyKeyingMaterial(iutSecretKeyingMaterial);
            var serverContribution = serverKas.Scheme.ThisPartyKeyingMaterial;

            IKdfParameter kdfParam = null;
            if (_param.KdfConfiguration != null)
            {
                kdfParam = _param.KdfConfiguration.GetKdfParameter(_kdfParameterVisitor);
            }
            
            await Notify(new KasAftResultIfc()
            {
                ServerC = serverContribution.C,
                ServerK = serverContribution.K,
                ServerNonce = serverContribution.DkmNonce,
                ServerZ = serverContribution.Z,
                ServerKeyPair = _serverKeyPair,
                IutKeyPair = _iutKeyPair,
                KdfParameter = kdfParam
            });
        }
    }
}