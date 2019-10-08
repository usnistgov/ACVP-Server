using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Crypto.Common.KTS;
using NIST.CVP.Crypto.KAS.Fakes;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Math.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Kas;
using System;
using System.Linq;
using System.Threading.Tasks;

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
        private readonly IKdfVisitor _kdfVisitor;
        private readonly IKdfParameterVisitor _kdfParameterVisitor;
        private readonly IKtsFactory _ktsFactory;
        private readonly IKeyConfirmationFactory _keyConfirmationFactory;
        private readonly IFixedInfoFactory _fixedInfoFactory;
        private readonly IEntropyProvider _entropyProvider;
        private readonly IRsaSve _rsaSve;
        private readonly IRsaSveBuilder _rsaSveBuilder;
        private readonly IRsa _rsa;
        private readonly IRandom800_90 _random;

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
            IKdfVisitor kdfVisitor,
            IKdfParameterVisitor kdfParameterVisitor,
            IKtsFactory ktsFactory,
            IKeyConfirmationFactory keyConfirmationFactory,
            IFixedInfoFactory fixedInfoFactory,
            IEntropyProvider entropyProvider,
            IRsaSve rsaSve,
            IRsaSveBuilder rsaSveBuilder,
            IRsa rsa,
            IRandom800_90 random
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
            _kdfVisitor = kdfVisitor;
            _kdfParameterVisitor = kdfParameterVisitor;
            _ktsFactory = ktsFactory;
            _keyConfirmationFactory = keyConfirmationFactory;
            _fixedInfoFactory = fixedInfoFactory;
            _entropyProvider = entropyProvider;
            _rsaSve = rsaSve;
            _rsaSveBuilder = rsaSveBuilder;
            _rsa = rsa;
            _random = random;
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

                IRsaSve rsaSve = _rsaSve;
                IEntropyProvider entropyProvider = _entropyProvider;

                var fixedInfoParameter = new FixedInfoParameter()
                {
                    L = _param.L,
                    // vvv These are set internally to the kas instance vvv
                    FixedInfoPartyU = null,
                    FixedInfoPartyV = null
                    // ^^^ These are set internally to the kas instance ^^^
                };

                // Handles setting up z value with a leading 0 nibble. 
                if (_param.Disposition == KasIfcValTestDisposition.SuccessLeadingZeroNibbleZ)
                {
                    var leadingZeroesEntropyFactory = new EntropyProviderLeadingZeroesFactory(_random)
                    {
                        MinimumLeadingZeroes = 4 // first nibble zero
                    };

                    entropyProvider = leadingZeroesEntropyFactory.GetEntropyProvider(EntropyProviderTypes.Random);

                    rsaSve = _rsaSveBuilder
                        .WithRsa(_rsa)
                        .WithEntropyProvider(entropyProvider)
                        .Build();
                }

                // KDF
                IKdfFactory kdfFactory = _kdfFactory;
                if (_param.Disposition == KasIfcValTestDisposition.FailChangedZ)
                {
                    kdfFactory = new KdfFactory(new FakeKdfVisitor_BadZ(_kdfVisitor, _random));
                    testPassed = false;
                }
                if (_param.Disposition == KasIfcValTestDisposition.FailChangedDkm)
                {
                    kdfFactory = new KdfFactory(new FakeKdfVisitor_BadDkm(_kdfVisitor, _random));
                    testPassed = false;
                }

                IKdfParameter kdfParam = null;
                if (KeyGenerationRequirementsHelper.IfcKdfSchemes.Contains(_param.Scheme))
                {
                    kdfParam = _param.KdfConfiguration.GetKdfParameter(_kdfParameterVisitor);

                    fixedInfoParameter.Encoding = _param.KdfConfiguration.FixedInputEncoding;
                    fixedInfoParameter.FixedInfoPattern = _param.KdfConfiguration.FixedInputPattern;
                    fixedInfoParameter.Salt = kdfParam.Salt;
                    fixedInfoParameter.Iv = kdfParam.Iv;
                    fixedInfoParameter.Label = kdfParam.Label;
                    fixedInfoParameter.Context = kdfParam.Context;
                    fixedInfoParameter.AlgorithmId = kdfParam.AlgorithmId;
                }

                // KTS
                IKtsFactory ktsFactory = _ktsFactory;
                if (_param.Disposition == KasIfcValTestDisposition.FailChangedZ)
                {
                    ktsFactory = new FakeKtsFactory_BadZ(_ktsFactory, _random);
                    testPassed = false;
                }

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

                    if (_param.Disposition == KasIfcValTestDisposition.FailChangedMacData)
                    {
                        kcFactory = new FakeKeyConfirmationFactory_BadMacData();
                        testPassed = false;
                    }
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
                    .WithKdf(kdfFactory, kdfParam)
                    .WithKts(ktsFactory, ktsParam)
                    .WithKeyConfirmation(kcFactory, macParam)
                    .WithRsaSve(rsaSve)
                    .WithEntropyProvider(entropyProvider)
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
                    .WithKdf(kdfFactory, kdfParam)
                    .WithKts(ktsFactory, ktsParam)
                    .WithKeyConfirmation(kcFactory, macParam)
                    .WithRsaSve(rsaSve)
                    .WithEntropyProvider(entropyProvider)
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

                var response = new KasValResultIfc()
                {
                    IutC = isServerPartyV ? result.KeyingMaterialPartyU.C : result.KeyingMaterialPartyV.C,
                    IutK = isServerPartyV ? result.KeyingMaterialPartyU.K : result.KeyingMaterialPartyV.K,
                    IutNonce = isServerPartyV
                        ? result.KeyingMaterialPartyU.DkmNonce
                        : result.KeyingMaterialPartyV.DkmNonce,
                    IutZ = isServerPartyV ? result.KeyingMaterialPartyU.Z : result.KeyingMaterialPartyV.Z,
                    IutKeyPair = isServerPartyV ? result.KeyingMaterialPartyU.Key : result.KeyingMaterialPartyV.Key,

                    ServerC = isServerPartyU ? result.KeyingMaterialPartyU.C : result.KeyingMaterialPartyV.C,
                    ServerK = isServerPartyU ? result.KeyingMaterialPartyU.K : result.KeyingMaterialPartyV.K,
                    ServerNonce = isServerPartyU
                        ? result.KeyingMaterialPartyU.DkmNonce
                        : result.KeyingMaterialPartyV.DkmNonce,
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
                };

                if (_param.Disposition == KasIfcValTestDisposition.FailKeyConfirmationBits)
                {
                    var newDkm = result.MacKey.ConcatenateBits(result.Dkm);

                    response.KasResult = new KasResult(
                        newDkm, result.MacKey, result.MacData, result.Tag);
                    response.TestPassed = false;
                }
                if (_param.Disposition == KasIfcValTestDisposition.FailChangedDkm)
                {
                    var dkmByteLen = response.KasResult.Dkm.BitLength.CeilingDivide(BitString.BITSINBYTE);
                    response.KasResult.Dkm[_random.GetRandomInt(0, dkmByteLen)] += 2;
                    response.TestPassed = false;
                }
                if (_param.Disposition == KasIfcValTestDisposition.FailChangedTag)
                {
                    var tagByteLen = response.KasResult.Tag.BitLength.CeilingDivide(BitString.BITSINBYTE);
                    response.KasResult.Tag[_random.GetRandomInt(0, tagByteLen)] += 2;
                    response.TestPassed = false;
                }

                await Notify(response);
            }
            catch (Exception e)
            {
                await Throw(e);
            }
        }
    }
}