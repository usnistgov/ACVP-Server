using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Builders;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kas.Sp800_56Ar3;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kas.Sp800_56Ar3
{
    public class ObserverKasSscCompleteDeferredAftGrain : ObservableOracleGrainBase<KasSscAftDeferredResult>,
        IObserverKasSscCompleteDeferredAftGrain
    {
        private readonly IKasBuilder _kasBuilder;
        private readonly ISchemeBuilder _schemeBuilder;
        private readonly ISecretKeyingMaterialBuilder _serverSecretKeyingMaterialBuilder;
        private readonly ISecretKeyingMaterialBuilder _iutSecretKeyingMaterialBuilder;
        private readonly IShaFactory _shaFactory;

        private KasSscAftDeferredParameters _param;

        public ObserverKasSscCompleteDeferredAftGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IKasBuilder kasBuilder,
            ISchemeBuilder schemeBuilder,
            ISecretKeyingMaterialBuilder serverSecretKeyingMaterialBuilder,
            ISecretKeyingMaterialBuilder iutSecretKeyingMaterialBuilder,
            IShaFactory shaFactory
        ) : base(nonOrleansScheduler)
        {
            _kasBuilder = kasBuilder;
            _schemeBuilder = schemeBuilder;
            _serverSecretKeyingMaterialBuilder = serverSecretKeyingMaterialBuilder;
            _iutSecretKeyingMaterialBuilder = iutSecretKeyingMaterialBuilder;
            _shaFactory = shaFactory;
        }

        public async Task<bool> BeginWorkAsync(KasSscAftDeferredParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            try
            {
                // depending if the server is party U or party V in the negotiation.
                var isServerPartyU = _param.ServerGenerationRequirements.ThisPartyKasRole == KeyAgreementRole.InitiatorPartyU;
                var isServerPartyV = !isServerPartyU;

                _serverSecretKeyingMaterialBuilder
                    .WithDomainParameters(_param.DomainParameters)
                    .WithEphemeralKey(_param.EphemeralKeyServer)
                    .WithStaticKey(_param.StaticKeyServer);

                var serverSecretKeyingMaterial = _serverSecretKeyingMaterialBuilder
                    .Build(
                        _param.KasScheme,
                        _param.ServerGenerationRequirements.KasMode,
                        _param.ServerGenerationRequirements.ThisPartyKasRole,
                        _param.ServerGenerationRequirements.ThisPartyKeyConfirmationRole,
                        _param.ServerGenerationRequirements.KeyConfirmationDirection);

                _iutSecretKeyingMaterialBuilder
                    .WithDomainParameters(_param.DomainParameters)
                    .WithEphemeralKey(_param.EphemeralKeyIut)
                    .WithStaticKey(_param.StaticKeyIut);

                var iutSecretKeyingMaterial = _iutSecretKeyingMaterialBuilder
                    .Build(
                        _param.KasScheme,
                        _param.IutGenerationRequirements.KasMode,
                        _param.IutGenerationRequirements.ThisPartyKasRole,
                        _param.IutGenerationRequirements.ThisPartyKeyConfirmationRole,
                        _param.IutGenerationRequirements.KeyConfirmationDirection);

                _schemeBuilder
                    .WithSchemeParameters(
                        new SchemeParameters(
                            new KasAlgoAttributes(_param.KasScheme),
                            _param.ServerGenerationRequirements.ThisPartyKasRole,
                            _param.ServerGenerationRequirements.KasMode,
                            _param.ServerGenerationRequirements.ThisPartyKeyConfirmationRole,
                            _param.ServerGenerationRequirements.KeyConfirmationDirection,
                            KasAssurance.None,
                            null))
                    .WithThisPartyKeyingMaterial(serverSecretKeyingMaterial);

                var serverKas = _kasBuilder.WithSchemeBuilder(_schemeBuilder).Build();

                var result = serverKas.ComputeResult(iutSecretKeyingMaterial);

                // Some implementations can't output the computed Z in the clear, hash it and add that to the returned result 
                if (_param.HashFunctionZ != HashFunctions.None)
                {
                    var hashFunction = ShaAttributes.GetHashFunctionFromEnum(_param.HashFunctionZ);
                    var sha = _shaFactory.GetShaInstance(hashFunction);
                    var hashZ = sha.HashMessage(result.Z).Digest;

                    result = new KeyAgreementResult(
                        result.SecretKeyingMaterialPartyU,
                        result.SecretKeyingMaterialPartyV,
                        result.Z,
                        hashZ);
                }

                var returnResult = new KasSscAftDeferredResult()
                {
                    ServerSecretKeyingMaterial = isServerPartyU ? result.SecretKeyingMaterialPartyU : result.SecretKeyingMaterialPartyV,
                    IutSecretKeyingMaterial = isServerPartyV ? result.SecretKeyingMaterialPartyV : result.SecretKeyingMaterialPartyU,
                    KasResult = result
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
