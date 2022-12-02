using System;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
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
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Br2;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Br2;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kas.Sp800_56Br2;
using KasAftDeferredResult = NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Br2.KasAftDeferredResult;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kas.Sp800_56Br2
{
    public class ObserverKasSscCompleteDeferredAftIfcCaseGrain : ObservableOracleGrainBase<KasSscAftDeferredResultIfc>,
        IObserverKasSscCompleteDeferredAftIfcCaseGrain
    {
        private readonly IKasIfcBuilder _kasBuilder;
        private readonly ISchemeIfcBuilder _schemeBuilder;
        private readonly IIfcSecretKeyingMaterialBuilder _serverSecretKeyingMaterialBuilder;
        private readonly IIfcSecretKeyingMaterialBuilder _iutSecretKeyingMaterialBuilder;
        private readonly IEntropyProvider _entropyProvider;
        private readonly IRsaSve _rsaSve;
        private readonly IShaFactory _shaFactory;

        private KasSscAftDeferredParametersIfc _param;

        public ObserverKasSscCompleteDeferredAftIfcCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IKasIfcBuilder kasBuilder,
            ISchemeIfcBuilder schemeBuilder,
            IIfcSecretKeyingMaterialBuilder serverSecretKeyingMaterialBuilder,
            IIfcSecretKeyingMaterialBuilder iutSecretKeyingMaterialBuilder,
            IEntropyProvider entropyProvider,
            IRsaSve rsaSve,
            IShaFactory shaFactory
        )
            : base(nonOrleansScheduler)
        {
            _kasBuilder = kasBuilder;
            _schemeBuilder = schemeBuilder;
            _serverSecretKeyingMaterialBuilder = serverSecretKeyingMaterialBuilder;
            _iutSecretKeyingMaterialBuilder = iutSecretKeyingMaterialBuilder;
            _entropyProvider = entropyProvider;
            _rsaSve = rsaSve;
            _shaFactory = shaFactory;
        }

        public async Task<bool> BeginWorkAsync(KasSscAftDeferredParametersIfc param)
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

                _serverSecretKeyingMaterialBuilder
                    .WithKey(_param.ServerKey)
                    .WithC(_param.ServerC)
                    .WithZ(_param.ServerZ);

                var serverSecretKeyingMaterial = _serverSecretKeyingMaterialBuilder
                    .Build(
                        mappedScheme,
                        _param.KasMode,
                        _param.ServerKeyAgreementRole,
                        KeyConfirmationRole.None,
                        KeyConfirmationDirection.None);

                var iutSecretKeyingMaterial = _iutSecretKeyingMaterialBuilder
                    .WithKey(_param.IutKey)
                    .WithC(_param.IutC)
                    .WithZ(_param.IutZ)
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
                    .WithThisPartyKeyingMaterial(serverSecretKeyingMaterial)
                    .WithRsaSve(_rsaSve)
                    .WithEntropyProvider(_entropyProvider);

                var serverKas = _kasBuilder.WithSchemeBuilder(_schemeBuilder).Build();

                var result = serverKas.ComputeResult(iutSecretKeyingMaterial);

                BitString hashZ = null;
                if (_param.HashFunctionZ != HashFunctions.None)
                {
                    var hashFunction = ShaAttributes.GetHashFunctionFromEnum(_param.HashFunctionZ);
                    var sha = _shaFactory.GetShaInstance(hashFunction);
                    hashZ = sha.HashMessage(result.Dkm).Digest;
                }

                var returnResult = new KasSscAftDeferredResultIfc()
                {
                    ServerKeyingMaterial = isServerPartyU ? result.KeyingMaterialPartyU : result.KeyingMaterialPartyV,
                    IutKeyingMaterial = isServerPartyV ? result.KeyingMaterialPartyU : result.KeyingMaterialPartyV,
                    Result = new KasResult(result.Dkm),
                    HashZ = hashZ
                };

                await Notify(returnResult);
            }
            catch (Exception e)
            {
                await Throw(e);
            }
        }
    }
}
