using System;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KTS;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Fakes;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Br2;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Br2;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kas.Sp800_56Br2;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kas.Sp800_56Br2
{
    public class ObserverKasSscValIfcCaseGrain : ObservableOracleGrainBase<KasSscValResultIfc>,
    IObserverKasSscValIfcCaseGrain
    {
        private readonly IKasIfcBuilder _kasBuilderPartyU;
        private readonly ISchemeIfcBuilder _schemeBuilderPartyU;
        private readonly IKasIfcBuilder _kasBuilderPartyV;
        private readonly ISchemeIfcBuilder _schemeBuilderPartyV;
        private readonly IIfcSecretKeyingMaterialBuilder _secretKeyingMaterialBuilderPartyU;
        private readonly IIfcSecretKeyingMaterialBuilder _secretKeyingMaterialBuilderPartyV;
        private readonly IEntropyProvider _entropyProvider;
        private readonly IRsaSve _rsaSve;
        private readonly IRsaSveBuilder _rsaSveBuilder;
        private readonly IRsa _rsa;
        private readonly IRandom800_90 _random;
        private readonly IShaFactory _shaFactory;

        private KasSscValParametersIfc _param;

        public ObserverKasSscValIfcCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IKasIfcBuilder kasBuilderPartyU,
            ISchemeIfcBuilder schemeBuilderPartyU,
            IKasIfcBuilder kasBuilderPartyV,
            ISchemeIfcBuilder schemeBuilderPartyV,
            IIfcSecretKeyingMaterialBuilder secretKeyingMaterialBuilderPartyU,
            IIfcSecretKeyingMaterialBuilder secretKeyingMaterialBuilderPartyV,
            IEntropyProvider entropyProvider,
            IRsaSve rsaSve,
            IRsaSveBuilder rsaSveBuilder,
            IRsa rsa,
            IRandom800_90 random,
            IShaFactory shaFactory
        )
            : base(nonOrleansScheduler)
        {
            _kasBuilderPartyU = kasBuilderPartyU;
            _schemeBuilderPartyU = schemeBuilderPartyU;
            _kasBuilderPartyV = kasBuilderPartyV;
            _schemeBuilderPartyV = schemeBuilderPartyV;
            _secretKeyingMaterialBuilderPartyU = secretKeyingMaterialBuilderPartyU;
            _secretKeyingMaterialBuilderPartyV = secretKeyingMaterialBuilderPartyV;
            _entropyProvider = entropyProvider;
            _rsaSve = rsaSve;
            _rsaSveBuilder = rsaSveBuilder;
            _rsa = rsa;
            _random = random;
            _shaFactory = shaFactory;
        }

        public async Task<bool> BeginWorkAsync(KasSscValParametersIfc param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var mappedScheme = _param.Scheme == SscIfcScheme.Kas1 ? IfcScheme.Kas1_basic : IfcScheme.Kas2_basic;

            try
            {
                var isServerPartyU = _param.ServerKeyAgreementRole == KeyAgreementRole.InitiatorPartyU;
                var isServerPartyV = !isServerPartyU;

                // Set the key and party id for party u's builder
                _secretKeyingMaterialBuilderPartyU
                    .WithKey(isServerPartyU ? _param.ServerKeyPair : _param.IutKeyPair);

                // Partial party v secret keying material is needed for party U to initialize
                var secretKeyingMaterialPartyV = _secretKeyingMaterialBuilderPartyV
                    .WithKey(isServerPartyV ? _param.ServerKeyPair : _param.IutKeyPair)
                    .Build(
                        mappedScheme,
                        _param.KasMode,
                        isServerPartyV ? _param.ServerKeyAgreementRole : _param.IutKeyAgreementRole,
                        KeyConfirmationRole.None,
                        KeyConfirmationDirection.None,
                        false);

                IRsaSve rsaSve = _rsaSve;
                IEntropyProvider entropyProvider = _entropyProvider;

                // Handles setting up z value with a leading 0 nibble. 
                if (_param.Disposition == KasSscTestCaseExpectation.PassLeadingZeroNibble)
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

                // Initialize Party U KAS
                _schemeBuilderPartyU
                    .WithSchemeParameters(new SchemeParametersIfc(
                        new KasAlgoAttributesIfc(mappedScheme, _param.Modulo, 0),
                        isServerPartyU ? _param.ServerKeyAgreementRole : _param.IutKeyAgreementRole,
                        _param.KasMode,
                        KeyConfirmationRole.None,
                        KeyConfirmationDirection.None,
                        KasAssurance.None,
                        null
                    ))
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
                        new KasAlgoAttributesIfc(mappedScheme, _param.Modulo, 0),
                        isServerPartyV ? _param.ServerKeyAgreementRole : _param.IutKeyAgreementRole,
                        _param.KasMode,
                        KeyConfirmationRole.None,
                        KeyConfirmationDirection.None,
                        KasAssurance.None,
                        null
                    ))
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

                var response = new KasSscValResultIfc()
                {
                    IutC = isServerPartyV ? result.KeyingMaterialPartyU.C : result.KeyingMaterialPartyV.C,
                    IutZ = isServerPartyV ? result.KeyingMaterialPartyU.Z : result.KeyingMaterialPartyV.Z,
                    IutKeyPair = isServerPartyV ? result.KeyingMaterialPartyU.Key : result.KeyingMaterialPartyV.Key,

                    ServerC = isServerPartyU ? result.KeyingMaterialPartyU.C : result.KeyingMaterialPartyV.C,
                    ServerZ = isServerPartyU ? result.KeyingMaterialPartyU.Z : result.KeyingMaterialPartyV.Z,
                    ServerKeyPair = isServerPartyU ? result.KeyingMaterialPartyU.Key : result.KeyingMaterialPartyV.Key,

                    KasResult = new KasResult(result.Dkm, result.MacKey, result.MacData, result.Tag),

                    Z = new BitString(0)
                        .ConcatenateBits(result.KeyingMaterialPartyU.Z ?? new BitString(0))
                        .ConcatenateBits(result.KeyingMaterialPartyV.Z ?? new BitString(0)),

                    TestPassed = true,
                    Disposition = _param.Disposition,
                };

                if (_param.Disposition == KasSscTestCaseExpectation.FailChangedZ)
                {
                    if (response.ServerZ != null)
                    {
                        var zByteLen = response.ServerZ.BitLength.CeilingDivide(BitString.BITSINBYTE);
                        response.ServerZ[_random.GetRandomInt(0, zByteLen)] += 2;
                    }

                    if (response.IutZ != null)
                    {
                        var zByteLen = response.IutZ.BitLength.CeilingDivide(BitString.BITSINBYTE);
                        response.IutZ[_random.GetRandomInt(0, zByteLen)] += 2;
                    }

                    var zPartyU = isServerPartyU ? response.ServerZ : response.IutZ;
                    var zPartyV = isServerPartyV ? response.IutZ : response.ServerZ;

                    response.Z = new BitString(0)
                        .ConcatenateBits(zPartyU ?? new BitString(0))
                        .ConcatenateBits(zPartyV ?? new BitString(0));

                    response.TestPassed = false; 
                }

                if (_param.HashFunctionZ != HashFunctions.None)
                {
                    var hashFunction = ShaAttributes.GetHashFunctionFromEnum(_param.HashFunctionZ);
                    var sha = _shaFactory.GetShaInstance(hashFunction);
                    response.HashZ = sha.HashMessage(response.Z).Digest;
                    response.IutHashZ = response.IutZ != null ? sha.HashMessage(response.IutZ).Digest : null;
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
