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
    public class ObserverKasSscAftIfcCaseGrain : ObservableOracleGrainBase<KasSscAftResultIfc>, IObserverKasSscAftIfcCaseGrain
    {
        private readonly IKasIfcBuilder _kasBuilder;
        private readonly ISchemeIfcBuilder _schemeBuilder;
        private readonly IIfcSecretKeyingMaterialBuilder _serverSecretKeyingMaterialBuilder;
        private readonly IIfcSecretKeyingMaterialBuilder _iutSecretKeyingMaterialBuilder;
        private readonly IEntropyProvider _entropyProvider;
        private readonly IRsaSve _rsaSve;

        private KasSscAftParametersIfc _param;

        public ObserverKasSscAftIfcCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IKasIfcBuilder kasBuilder,
            ISchemeIfcBuilder schemeBuilder,
            IIfcSecretKeyingMaterialBuilder serverSecretKeyingMaterialBuilder,
            IIfcSecretKeyingMaterialBuilder iutSecretKeyingMaterialBuilder,
            IEntropyProvider entropyProvider,
            IRsaSve rsaSve)
            : base(nonOrleansScheduler)
        {
            _kasBuilder = kasBuilder;
            _schemeBuilder = schemeBuilder;
            _serverSecretKeyingMaterialBuilder = serverSecretKeyingMaterialBuilder;
            _iutSecretKeyingMaterialBuilder = iutSecretKeyingMaterialBuilder;
            _entropyProvider = entropyProvider;
            _rsaSve = rsaSve;
        }

        public async Task<bool> BeginWorkAsync(KasSscAftParametersIfc param)
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
                _serverSecretKeyingMaterialBuilder
                    .WithKey(_param.ServerKeyPair);

                var iutSecretKeyingMaterial = _iutSecretKeyingMaterialBuilder
                    .WithKey(_param.IutKeyPair)
                    .Build(
                        mappedScheme,
                        _param.KasMode,
                        _param.IutKeyAgreementRole,
                        KeyConfirmationRole.None,
                        KeyConfirmationDirection.None);

                _schemeBuilder
                    .WithSchemeParameters(
                        new SchemeParametersIfc(
                            new KasAlgoAttributesIfc(mappedScheme, _param.Modulo, 0),
                            _param.ServerKeyAgreementRole,
                            _param.KasMode,
                            KeyConfirmationRole.None,
                            KeyConfirmationDirection.None,
                            KasAssurance.None,
                            null))
                    .WithThisPartyKeyingMaterialBuilder(_serverSecretKeyingMaterialBuilder)
                    .WithRsaSve(_rsaSve)
                    .WithEntropyProvider(_entropyProvider);

                var serverKas = _kasBuilder.WithSchemeBuilder(_schemeBuilder).Build();
                serverKas.InitializeThisPartyKeyingMaterial(iutSecretKeyingMaterial);
                var serverContribution = serverKas.Scheme.ThisPartyKeyingMaterial;

                await Notify(new KasSscAftResultIfc()
                {
                    ServerC = serverContribution.C,
                    ServerZ = serverContribution.Z,
                    ServerKeyPair = _param.ServerKeyPair,
                    IutKeyPair = _param.IutKeyPair,
                });
            }
            catch (Exception e)
            {
                await Throw(e);
            }
        }
    }
}
