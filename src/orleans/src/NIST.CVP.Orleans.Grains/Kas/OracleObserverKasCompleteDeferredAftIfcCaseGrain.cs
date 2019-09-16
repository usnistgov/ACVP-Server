using System;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KTS;
using NIST.CVP.Orleans.Grains.Interfaces.Kas;

namespace NIST.CVP.Orleans.Grains.Kas
{
    public class OracleObserverKasCompleteDeferredAftIfcCaseGrain : ObservableOracleGrainBase<KasAftDeferredResult>, 
        IOracleObserverKasCompleteDeferredAftIfcCaseGrain
    {
        private readonly IKasIfcBuilder _kasBuilder;
        private readonly ISchemeIfcBuilder _schemeBuilder;
        private readonly IIfcSecretKeyingMaterialBuilder _serverSecretKeyingMaterialBuilder;
        private readonly IIfcSecretKeyingMaterialBuilder _iutSecretKeyingMaterialBuilder;
        private readonly IKdfFactory _kdfFactory;
        private readonly IKtsFactory _ktsFactory;
        private readonly IKeyConfirmationFactory _keyConfirmationFactory;
        private readonly IFixedInfoFactory _fixedInfoFactory;
        
        private KasAftDeferredParametersIfc _param;
        
        public OracleObserverKasCompleteDeferredAftIfcCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IKasIfcBuilder kasBuilder,
            ISchemeIfcBuilder schemeBuilder,
            IIfcSecretKeyingMaterialBuilder serverSecretKeyingMaterialBuilder,
            IIfcSecretKeyingMaterialBuilder iutSecretKeyingMaterialBuilder,
            IKdfFactory kdfFactory,
            IKtsFactory ktsFactory,
            IKeyConfirmationFactory keyConfirmationFactory,
            IFixedInfoFactory fixedInfoFactory
        ) 
            : base(nonOrleansScheduler)
        {
            _kasBuilder = kasBuilder;
            _schemeBuilder = schemeBuilder;
            _serverSecretKeyingMaterialBuilder = serverSecretKeyingMaterialBuilder;
            _iutSecretKeyingMaterialBuilder = iutSecretKeyingMaterialBuilder;
            _kdfFactory = kdfFactory;
            _ktsFactory = ktsFactory;
            _keyConfirmationFactory = keyConfirmationFactory;
            _fixedInfoFactory = fixedInfoFactory;
        }

        public async Task<bool> BeginWorkAsync(KasAftDeferredParametersIfc param)
        {
            _param = param;
            
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            _serverSecretKeyingMaterialBuilder
                .WithPartyId(_param.ServerPartyId)
                .WithKey(_param.ServerKey)
                .WithC(_param.ServerC)
                .WithDkmNonce(_param.ServerNonce);

            var iutSecretKeyingMaterial = _iutSecretKeyingMaterialBuilder
                .WithPartyId(_param.IutPartyId)
                .WithKey(_param.IutKey)
                .WithC(_param.IutC)
                .WithDkmNonce(_param.IutNonce)
                .Build(
                    _param.Scheme, 
                    _param.KasMode, 
                    _param.IutKeyAgreementRole, 
                    _param.IutKeyConfirmationRole,
                    _param.KeyConfirmationDirection);
            
            
            var fixedInfoParameter = new FixedInfoParameter()
            {
                L = _param.L,
                // vvv These are set internally to the kas instance vvv
                FixedInfoPartyU = null,
                FixedInfoPartyV = null
                // ^^^ These are set internally to the kas instance ^^^
            };
            
            // KDF fixed info construction
            if (KeyGenerationRequirementsHelper.IfcKdfSchemes.Contains(_param.Scheme))
            {
                fixedInfoParameter.Encoding = _param.KdfParameter.FixedInputEncoding;
                fixedInfoParameter.FixedInfoPattern = _param.KdfParameter.FixedInfoPattern;
            }
            
            // KTS fixed info construction
            if (KeyGenerationRequirementsHelper.IfcKtsSchemes.Contains(_param.Scheme))
            {
                fixedInfoParameter.Encoding = _param.KtsParameter.Encoding;
                fixedInfoParameter.FixedInfoPattern = _param.KtsParameter.AssociatedDataPattern;
            }

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
                .WithFixedInfo(_fixedInfoFactory, fixedInfoParameter)
                .WithKdf(_kdfFactory, _param.KdfParameter)
                .WithKts(_ktsFactory, _param.KtsParameter)
                .WithKeyConfirmation(_keyConfirmationFactory, _param.MacParameter);
                
            var serverKas = _kasBuilder.WithSchemeBuilder(_schemeBuilder).Build();
            serverKas.InitializeThisPartyKeyingMaterial(iutSecretKeyingMaterial);

            var result = serverKas.ComputeResult(iutSecretKeyingMaterial);
            
            await Notify(new KasAftDeferredResult()
            {
                Result = new KasResult(result.Dkm, null, null, result.Tag)
            });
        }
    }
}