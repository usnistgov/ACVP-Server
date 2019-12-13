using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Builders;
using NIST.CVP.Orleans.Grains.Interfaces.Kas.Sp800_56Ar3;


namespace NIST.CVP.Orleans.Grains.Kas.Sp800_56Ar3
{
    public class ObserverKasCompleteDeferredAftGrain : ObservableOracleGrainBase<KasAftDeferredResult>, 
        IObserverKasCompleteDeferredAftGrain
    {
        private readonly IKasBuilder _kasBuilder;
        private readonly ISchemeBuilder _schemeBuilder;
        private readonly ISecretKeyingMaterialBuilder _serverSecretKeyingMaterialBuilder;
        private readonly ISecretKeyingMaterialBuilder _iutSecretKeyingMaterialBuilder;
        private readonly IKdfFactory _kdfFactory; 
        private readonly IKeyConfirmationFactory _keyConfirmationFactory;
        private readonly IFixedInfoFactory _fixedInfoFactory;
        
        private KasAftDeferredParameters _param;
        
        public ObserverKasCompleteDeferredAftGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IKasBuilder kasBuilder,
            ISchemeBuilder schemeBuilder,
            ISecretKeyingMaterialBuilder serverSecretKeyingMaterialBuilder,
            ISecretKeyingMaterialBuilder iutSecretKeyingMaterialBuilder,
            IKdfFactory kdfFactory,
            IKeyConfirmationFactory keyConfirmationFactory,
            IFixedInfoFactory fixedInfoFactory
        ) : base (nonOrleansScheduler)
        {
            _kasBuilder = kasBuilder;
            _schemeBuilder = schemeBuilder;
            _serverSecretKeyingMaterialBuilder = serverSecretKeyingMaterialBuilder;
            _iutSecretKeyingMaterialBuilder = iutSecretKeyingMaterialBuilder;
            _kdfFactory = kdfFactory;
            _keyConfirmationFactory = keyConfirmationFactory;
            _fixedInfoFactory = fixedInfoFactory;
        }

        public async Task<bool> BeginWorkAsync(KasAftDeferredParameters param)
        {
            _param = param;
            
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            // depending if the server is party U or party V in the negotiation.
            var isServerPartyU = _param.ServerGenerationRequirements.ThisPartyKasRole == KeyAgreementRole.InitiatorPartyU;
            var isServerPartyV = !isServerPartyU;

            _serverSecretKeyingMaterialBuilder
                .WithPartyId(_param.ServerPartyId)
                .WithEphemeralKey(_param.ServerEphemeralKey)
                .WithStaticKey(_param.ServerStaticKey)
                .WithEphemeralNonce(_param.ServerEphemeralNonce)
                .WithDkmNonce(_param.ServerDkmNonce);
                
            var serverSecretKeyingMaterial = _serverSecretKeyingMaterialBuilder
                .Build(
                    _param.KasScheme,
                    _param.ServerGenerationRequirements.KasMode,
                    _param.ServerGenerationRequirements.ThisPartyKasRole,
                    _param.ServerGenerationRequirements.ThisPartyKeyConfirmationRole,
                    _param.ServerGenerationRequirements.KeyConfirmationDirection);
                
            _iutSecretKeyingMaterialBuilder
                .WithPartyId(_param.IutPartyId)
                .WithEphemeralKey(_param.IutEphemeralKey)
                .WithStaticKey(_param.IutStaticKey)
                .WithEphemeralNonce(_param.IutEphemeralNonce)
                .WithDkmNonce(_param.IutDkmNonce);
                
            var iutSecretKeyingMaterial = _iutSecretKeyingMaterialBuilder
                .Build(
                    _param.KasScheme,
                    _param.IutGenerationRequirements.KasMode,
                    _param.IutGenerationRequirements.ThisPartyKasRole,
                    _param.IutGenerationRequirements.ThisPartyKeyConfirmationRole,
                    _param.IutGenerationRequirements.KeyConfirmationDirection);

            var fixedInfoParameter = new FixedInfoParameter
            {
                L = _param.L,
                Encoding = _param.KdfParameter.FixedInputEncoding,
                FixedInfoPattern = _param.KdfParameter.FixedInfoPattern,
                Salt = _param.KdfParameter.Salt,
                Iv = _param.KdfParameter.Iv,
                Label = _param.KdfParameter.Label,
                Context = _param.KdfParameter.Context,
                AlgorithmId = _param.KdfParameter.AlgorithmId,
            };

            // KDF fixed info construction

            MacParameters macParam = null;
            IKeyConfirmationFactory kcFactory = null;
            if (_param.ServerGenerationRequirements.ThisPartyKeyConfirmationRole != KeyConfirmationRole.None)
            {
                macParam = _param.MacParameter;
                kcFactory = _keyConfirmationFactory;
            }
            
            _schemeBuilder
                .WithSchemeParameters(
                    new SchemeParameters(
                        new KasAlgoAttributes(_param.KasScheme),
                        _param.ServerGenerationRequirements.ThisPartyKasRole,
                        _param.ServerGenerationRequirements.KasMode,
                        _param.ServerGenerationRequirements.ThisPartyKeyConfirmationRole,
                        _param.ServerGenerationRequirements.KeyConfirmationDirection,
                        KasAssurance.None,
                        _param.ServerPartyId))
                .WithThisPartyKeyingMaterial(serverSecretKeyingMaterial)
                .WithFixedInfo(_fixedInfoFactory, fixedInfoParameter)
                .WithKdf(_kdfFactory, _param.KdfParameter)
                .WithKeyConfirmation(kcFactory, macParam);

            var serverKas = _kasBuilder.WithSchemeBuilder(_schemeBuilder).Build();

            var result = serverKas.ComputeResult(iutSecretKeyingMaterial);
            var returnResult = new KasAftDeferredResult()
            {
                ServerSecretKeyingMaterial = isServerPartyU ? result.SecretKeyingMaterialPartyU : result.SecretKeyingMaterialPartyV,
                IutSecretKeyingMaterial = isServerPartyV ? result.SecretKeyingMaterialPartyV : result.SecretKeyingMaterialPartyU,
                KasResult = result
            };

            await Notify(returnResult);
        }
    }
}