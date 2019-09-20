using System;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KTS;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Kas;

namespace NIST.CVP.Orleans.Grains.Kas
{
    public class OracleObserverKasValIfcCaseGrain : ObservableOracleGrainBase<KasValResultIfc>, 
    IOracleObserverKasValIfcCaseGrain
    {
        private readonly IKasIfcBuilder _kasBuilderPartyU;
        private readonly ISchemeIfcBuilder _schemeBuilderPartyU;
        private readonly IKasIfcBuilder _kasBuilderPartyV;
        private readonly ISchemeIfcBuilder _schemeBuilderPartyV;
        private readonly IIfcSecretKeyingMaterialBuilder _secretKeyingMaterialBuilderPartyU;
        private readonly IIfcSecretKeyingMaterialBuilder _secretKeyingMaterialBuilderPartyV;
        private readonly IKdfFactory _kdfFactory;
        private readonly IKdfParameterVisitor _kdfParameterVisitor;
        private readonly IKtsFactory _ktsFactory;
        private readonly IKeyConfirmationFactory _keyConfirmationFactory;
        private readonly IFixedInfoFactory _fixedInfoFactory;
        
        private KasValParametersIfc _param;
        private KeyPair _serverKeyPair;
        private KeyPair _iutKeyPair;

        public OracleObserverKasValIfcCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IKasIfcBuilder kasBuilderPartyU,
            ISchemeIfcBuilder schemeBuilderPartyU,
            IKasIfcBuilder kasBuilderPartyV,
            ISchemeIfcBuilder schemeBuilderPartyV,
            IIfcSecretKeyingMaterialBuilder secretKeyingMaterialBuilderPartyU,
            IIfcSecretKeyingMaterialBuilder secretKeyingMaterialBuilderPartyV,
            IKdfFactory kdfFactory,
            IKdfParameterVisitor kdfParameterVisitor,
            IKtsFactory ktsFactory,
            IKeyConfirmationFactory keyConfirmationFactory,
            IFixedInfoFactory fixedInfoFactory
        ) 
            : base(nonOrleansScheduler)
        {
            _kasBuilderPartyU = kasBuilderPartyU;
            _schemeBuilderPartyU = schemeBuilderPartyU;
            _kasBuilderPartyV = kasBuilderPartyV;
            _schemeBuilderPartyV = schemeBuilderPartyV;
            _secretKeyingMaterialBuilderPartyU = secretKeyingMaterialBuilderPartyU;
            _secretKeyingMaterialBuilderPartyV = secretKeyingMaterialBuilderPartyV;
            _kdfFactory = kdfFactory;
            _kdfParameterVisitor = kdfParameterVisitor;
            _ktsFactory = ktsFactory;
            _keyConfirmationFactory = keyConfirmationFactory;
            _fixedInfoFactory = fixedInfoFactory;
        }

        public async Task<bool> BeginWorkAsync(KasValParametersIfc param, KeyPair serverKeyPair, KeyPair iutKeyPair)
        {
            _param = param;
            _serverKeyPair = serverKeyPair;
            _iutKeyPair = iutKeyPair;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            try
            {
                var isServerPartyU = _param.ServerKeyAgreementRole == KeyAgreementRole.InitiatorPartyU;
                var isServerPartyV = !isServerPartyU;
                var testPassed = true;

                // Set the key and party id for party u's builder
                _secretKeyingMaterialBuilderPartyU
                    .WithKey(isServerPartyU ? _serverKeyPair : _iutKeyPair)
                    .WithPartyId(isServerPartyU ? _param.ServerPartyId : _param.IutPartyId);
                
                // Partial party v secret keying material is needed for party U to initialize
                var secretKeyingMaterialPartyV = _secretKeyingMaterialBuilderPartyV
                    .WithKey(isServerPartyV ? _serverKeyPair : _iutKeyPair)
                    .WithPartyId(isServerPartyV ? _param.ServerPartyId : _param.IutPartyId)
                    .Build(
                        _param.Scheme, 
                        _param.KasMode, 
                        isServerPartyV ? _param.ServerKeyAgreementRole : _param.IutKeyAgreementRole, 
                        isServerPartyV ? _param.ServerKeyConfirmationRole : _param.IutKeyConfirmationRole,
                        _param.KeyConfirmationDirection, 
                        false);
                
                var fixedInfoParameter = new FixedInfoParameter()
                {
                    L = _param.L,
                    // vvv These are set internally to the kas instance vvv
                    FixedInfoPartyU = null,
                    FixedInfoPartyV = null
                    // ^^^ These are set internally to the kas instance ^^^
                };
                
                // KDF
                IKdfParameter kdfParam = null;
                if (KeyGenerationRequirementsHelper.IfcKdfSchemes.Contains(_param.Scheme))
                {
                    kdfParam = _param.KdfConfiguration.GetKdfParameter(_kdfParameterVisitor);
                    
                    fixedInfoParameter.Encoding = _param.KdfConfiguration.FixedInputEncoding;
                    fixedInfoParameter.FixedInfoPattern = _param.KdfConfiguration.FixedInputPattern;
                    fixedInfoParameter.Salt = kdfParam.Salt;
                }
                
                // KTS
                KtsParameter ktsParam = null;
                if (KeyGenerationRequirementsHelper.IfcKtsSchemes.Contains(_param.Scheme))
                {
                    fixedInfoParameter.Encoding = _param.KtsConfiguration.Encoding;
                    fixedInfoParameter.FixedInfoPattern = _param.KtsConfiguration.AssociatedDataPattern;
                    
                    ktsParam = new KtsParameter()
                    {
                        Encoding = _param.KtsConfiguration.Encoding,
                        AssociatedDataPattern = _param.KtsConfiguration.AssociatedDataPattern,
                        KtsHashAlg = _param.KtsConfiguration.KtsHashAlg
                    };
                }
                
                // MAC
                MacParameters macParam = null;
                IKeyConfirmationFactory kcFactory = null;
                if (KeyGenerationRequirementsHelper.IfcKcSchemes.Contains(_param.Scheme))
                {
                    macParam = new MacParameters(
                        _param.MacConfiguration.MacType, 
                        _param.MacConfiguration.KeyLen, 
                        _param.MacConfiguration.MacLen);

                    kcFactory = _keyConfirmationFactory;
                }
                
                // Initialize Party U KAS
                _schemeBuilderPartyU
                    .WithSchemeParameters(new SchemeParametersIfc(
                        new KasAlgoAttributesIfc(_param.Scheme, _param.Modulo, _param.L),
                        isServerPartyU ? _param.ServerKeyAgreementRole : _param.IutKeyAgreementRole,
                        _param.KasMode,
                        isServerPartyU ? _param.ServerKeyConfirmationRole : _param.IutKeyConfirmationRole,
                        _param.KeyConfirmationDirection,
                        KasAssurance.None,
                        isServerPartyU ? _param.ServerPartyId : _param.IutPartyId
                    ))
                    .WithFixedInfo(_fixedInfoFactory, fixedInfoParameter)
                    .WithKdf(_kdfFactory, kdfParam)
                    .WithKts(_ktsFactory, ktsParam)
                    .WithKeyConfirmation(kcFactory, macParam)
                    .WithThisPartyKeyingMaterialBuilder(_secretKeyingMaterialBuilderPartyU);

                var kasPartyU = _kasBuilderPartyU
                    .WithSchemeBuilder(_schemeBuilderPartyU)
                    .Build();
                    
                kasPartyU.InitializeThisPartyKeyingMaterial(secretKeyingMaterialPartyV);

                var initializedKeyingMaterialPartyU = kasPartyU.Scheme.ThisPartyKeyingMaterial;
                
                
                // Initialize Party V KAS
                _schemeBuilderPartyV
                    .WithSchemeParameters(new SchemeParametersIfc(
                        new KasAlgoAttributesIfc(_param.Scheme, _param.Modulo, _param.L),
                        isServerPartyV ? _param.ServerKeyAgreementRole : _param.IutKeyAgreementRole,
                        _param.KasMode,
                        isServerPartyV ? _param.ServerKeyConfirmationRole : _param.IutKeyConfirmationRole,
                        _param.KeyConfirmationDirection,
                        KasAssurance.None,
                        isServerPartyV ? _param.ServerPartyId : _param.IutPartyId
                    ))
                    .WithFixedInfo(_fixedInfoFactory, fixedInfoParameter)
                    .WithKdf(_kdfFactory, kdfParam)
                    .WithKts(_ktsFactory, ktsParam)
                    .WithKeyConfirmation(kcFactory, macParam)
                    .WithThisPartyKeyingMaterialBuilder(_secretKeyingMaterialBuilderPartyV);

                var kasPartyV = _kasBuilderPartyV
                    .WithSchemeBuilder(_schemeBuilderPartyV)
                    .Build();
                    
                kasPartyV.InitializeThisPartyKeyingMaterial(initializedKeyingMaterialPartyU);

                var initializedKeyingMaterialPartyV = kasPartyV.Scheme.ThisPartyKeyingMaterial;

                // Complete the KAS negotiation with whichever party is the IUT
                var result = isServerPartyU ? 
                    kasPartyV.ComputeResult(initializedKeyingMaterialPartyU) : 
                    kasPartyU.ComputeResult(initializedKeyingMaterialPartyV);
                
                await Notify(new KasValResultIfc()
                {
                    IutC = isServerPartyV ? result.KeyingMaterialPartyU.C : result.KeyingMaterialPartyV.C,
                    IutK = isServerPartyV ? result.KeyingMaterialPartyU.K : result.KeyingMaterialPartyV.K,
                    IutNonce = isServerPartyV ? result.KeyingMaterialPartyU.DkmNonce : result.KeyingMaterialPartyV.DkmNonce,
                    IutZ = isServerPartyV ? result.KeyingMaterialPartyU.Z : result.KeyingMaterialPartyV.Z,
                    IutKeyPair = isServerPartyV ? result.KeyingMaterialPartyU.Key : result.KeyingMaterialPartyV.Key,
                    
                    ServerC = isServerPartyU ? result.KeyingMaterialPartyU.C : result.KeyingMaterialPartyV.C,
                    ServerK = isServerPartyU ? result.KeyingMaterialPartyU.K : result.KeyingMaterialPartyV.K,
                    ServerNonce = isServerPartyU ? result.KeyingMaterialPartyU.DkmNonce : result.KeyingMaterialPartyV.DkmNonce,
                    ServerZ = isServerPartyU ? result.KeyingMaterialPartyU.Z : result.KeyingMaterialPartyV.Z,
                    ServerKeyPair = isServerPartyU ? result.KeyingMaterialPartyU.Key : result.KeyingMaterialPartyV.Key,
                    
                    KasResult = new KasResult(result.Dkm, result.MacKey, result.MacData, result.Tag),
                    KdfParameter = kdfParam,
                    KtsParameter = ktsParam,
                    MacParameters = macParam,
                    
                    Z = new BitString(0)
                        .ConcatenateBits(result.KeyingMaterialPartyU.Z ?? new BitString(0))
                        .ConcatenateBits(result.KeyingMaterialPartyV.Z ?? new BitString(0)),

                    TestPassed = testPassed,
                    Disposition = _param.Disposition,
                });
            }
            catch (Exception e)
            {
                await Throw(e);
            }
        }
    }
}