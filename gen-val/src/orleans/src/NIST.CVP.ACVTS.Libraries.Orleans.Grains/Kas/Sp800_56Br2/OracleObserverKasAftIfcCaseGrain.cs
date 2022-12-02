using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KTS;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Br2;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Br2;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kas.Sp800_56Br2;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kas.Sp800_56Br2
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
        private readonly IKtsFactory _ktsFactory;
        private readonly IFixedInfoFactory _fixedInfoFactory;

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
            IRsaSve rsaSve,
            IKtsFactory ktsFactory,
            IFixedInfoFactory fixedInfoFactory)
            : base(nonOrleansScheduler)
        {
            _kasBuilder = kasBuilder;
            _schemeBuilder = schemeBuilder;
            _serverSecretKeyingMaterialBuilder = serverSecretKeyingMaterialBuilder;
            _iutSecretKeyingMaterialBuilder = iutSecretKeyingMaterialBuilder;
            _kdfParameterVisitor = kdfParameterVisitor;
            _entropyProvider = entropyProvider;
            _rsaSve = rsaSve;
            _ktsFactory = ktsFactory;
            _fixedInfoFactory = fixedInfoFactory;
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
            try
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

                IKdfParameter kdfParam = null;
                if (_param.KdfConfiguration != null)
                {
                    kdfParam = _param.KdfConfiguration.GetKdfParameter(_kdfParameterVisitor);
                }

                KtsParameter ktsParameter = null;
                FixedInfoParameter fixedInfoParameter = null;
                if (_param.KtsConfiguration != null)
                {
                    const int bitsOfEntropy = 128;
                    var context =
                        _param.KtsConfiguration.AssociatedDataPattern.Contains(nameof(KtsParameter.Context),
                            StringComparison.OrdinalIgnoreCase)
                            ? _entropyProvider.GetEntropy(bitsOfEntropy)
                            : null;
                    var algorithmId =
                        _param.KtsConfiguration.AssociatedDataPattern.Contains(nameof(KtsParameter.AlgorithmId),
                            StringComparison.OrdinalIgnoreCase)
                            ? _entropyProvider.GetEntropy(bitsOfEntropy)
                            : null;
                    var label =
                        _param.KtsConfiguration.AssociatedDataPattern.Contains(nameof(KtsParameter.Label),
                            StringComparison.OrdinalIgnoreCase)
                            ? _entropyProvider.GetEntropy(bitsOfEntropy)
                            : null;

                    ktsParameter = new KtsParameter()
                    {
                        Encoding = _param.KtsConfiguration.Encoding,
                        AssociatedDataPattern = _param.KtsConfiguration.AssociatedDataPattern,
                        KtsHashAlg = _param.KtsConfiguration.HashAlg,
                        Context = context,
                        AlgorithmId = algorithmId,
                        Label = label
                    };
                    fixedInfoParameter = new FixedInfoParameter()
                    {
                        Encoding = _param.KtsConfiguration.Encoding,
                        FixedInfoPattern = _param.KtsConfiguration.AssociatedDataPattern,
                        Context = context,
                        AlgorithmId = algorithmId,
                        Label = label,
                        L = _param.L,
                    };
                }

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
                    .WithFixedInfo(_fixedInfoFactory, fixedInfoParameter)
                    .WithRsaSve(_rsaSve)
                    .WithKts(_ktsFactory, ktsParameter)
                    .WithEntropyProvider(_entropyProvider);

                var serverKas = _kasBuilder.WithSchemeBuilder(_schemeBuilder).Build();
                serverKas.InitializeThisPartyKeyingMaterial(iutSecretKeyingMaterial);
                var serverContribution = serverKas.Scheme.ThisPartyKeyingMaterial;

                await Notify(new KasAftResultIfc()
                {
                    ServerC = serverContribution.C,
                    ServerK = serverContribution.K,
                    ServerNonce = serverContribution.DkmNonce,
                    ServerZ = serverContribution.Z,
                    ServerKeyPair = _serverKeyPair,
                    IutKeyPair = _iutKeyPair,
                    KdfParameter = kdfParam,
                    KtsParameter = ktsParameter
                });
            }
            catch (Exception e)
            {
                await Throw(e);
            }
        }
    }
}
