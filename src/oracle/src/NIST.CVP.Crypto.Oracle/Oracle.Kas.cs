using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Kas;
using System.Threading.Tasks;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<KasValResultEcc> GetKasValTestEccAsync(KasValParametersEcc param)
        {
            var observableGrain =
                await GetObserverGrain<IOracleObserverKasValEccCaseGrain, KasValResultEcc>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<KasAftResultEcc> GetKasAftTestEccAsync(KasAftParametersEcc param)
        {
            var observableGrain =
                await GetObserverGrain<IOracleObserverKasAftEccCaseGrain, KasAftResultEcc>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersEcc param)
        {
            var observableGrain =
                await GetObserverGrain<IOracleObserverKasCompleteDeferredAftEccCaseGrain, KasAftDeferredResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<KasValResultFfc> GetKasValTestFfcAsync(KasValParametersFfc param)
        {
            var observableGrain =
                await GetObserverGrain<IOracleObserverKasValFfcCaseGrain, KasValResultFfc>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<KasAftResultFfc> GetKasAftTestFfcAsync(KasAftParametersFfc param)
        {
            var observableGrain =
                await GetObserverGrain<IOracleObserverKasAftFfcCaseGrain, KasAftResultFfc>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersFfc param)
        {
            var observableGrain =
                await GetObserverGrain<IOracleObserverKasCompleteDeferredAftFfcCaseGrain, KasAftDeferredResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<KasValResultIfc> GetKasValTestIfcAsync(KasValParametersIfc param)
        {
            // Get RSA keys on behalf of the server and IUT.
            var serverRequirements = KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                param.Scheme, param.KasMode, param.ServerKeyAgreementRole, param.ServerKeyConfirmationRole,
                param.KeyConfirmationDirection);

            Task<RsaKeyResult> serverKeyTask = null;
            KeyPair serverKey = null;
            if (serverRequirements.GeneratesEphemeralKeyPair)
            {
                serverKeyTask = GetRsaKeyAsync(new RsaKeyParameters()
                {
                    Modulus = param.Modulo,
                    KeyFormat = param.PrivateKeyMode,
                    KeyMode = PrimeGenModes.B33,
                    PrimeTest = PrimeTestModes.C2,
                    PublicExponentMode = param.PublicExponentMode,
                    PublicExponent = param.PublicExponent == 0 ? null : new BitString(param.PublicExponent)
                });
            }

            var iutRequirements = KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                param.Scheme, param.KasMode, param.IutKeyAgreementRole, param.IutKeyConfirmationRole,
                param.KeyConfirmationDirection);

            Task<RsaKeyResult> iutKeyTask = null;
            KeyPair iutKey = null;
            if (iutRequirements.GeneratesEphemeralKeyPair)
            {
                iutKeyTask = GetRsaKeyAsync(new RsaKeyParameters()
                {
                    Modulus = param.Modulo,
                    KeyFormat = param.PrivateKeyMode,
                    KeyMode = PrimeGenModes.B33,
                    PrimeTest = PrimeTestModes.C2,
                    PublicExponentMode = param.PublicExponentMode,
                    PublicExponent = param.PublicExponent == 0 ? null : new BitString(param.PublicExponent)
                });
            }

            if (serverKeyTask != null)
            {
                await serverKeyTask;
                serverKey = serverKeyTask.Result.Key;
            }
            if (iutKeyTask != null)
            {
                await iutKeyTask;
                iutKey = iutKeyTask.Result.Key;
            }

            var observableGrain =
                await GetObserverGrain<IOracleObserverKasValIfcCaseGrain, KasValResultIfc>();
            await GrainInvokeRetryWrapper.WrapGrainCall(
                observableGrain.Grain.BeginWorkAsync,
                param, serverKey, iutKey, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<KasAftResultIfc> GetKasAftTestIfcAsync(KasAftParametersIfc param)
        {
            // Get an RSA key on behalf of the server party when needed
            var serverRequirements = KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                param.Scheme, param.KasMode, param.ServerKeyAgreementRole, param.ServerKeyConfirmationRole,
                param.KeyConfirmationDirection);

            KeyPair serverKey = null;
            if (serverRequirements.GeneratesEphemeralKeyPair)
            {
                var task = await GetRsaKeyAsync(new RsaKeyParameters()
                {
                    Modulus = param.Modulo,
                    // it doesn't matter what format the key is in from the server perspective.
                    KeyFormat = PrivateKeyModes.Crt,
                    KeyMode = PrimeGenModes.B33,
                    PrimeTest = PrimeTestModes.C2,
                    PublicExponentMode = PublicExponentModes.Random,
                });
                serverKey = task.Key;
            }

            var observableGrain =
                await GetObserverGrain<IOracleObserverKasAftIfcCaseGrain, KasAftResultIfc>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, serverKey, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersIfc param)
        {
            var observableGrain =
                await GetObserverGrain<IOracleObserverKasCompleteDeferredAftIfcCaseGrain, KasAftDeferredResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<KasEccComponentResult> GetKasEccComponentTestAsync(KasEccComponentParameters param)
        {
            var observableGrain =
                await GetObserverGrain<IOracleObserverKasEccComponentCaseGrain, KasEccComponentResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<KasEccComponentDeferredResult> CompleteDeferredKasComponentTestAsync(KasEccComponentDeferredParameters param)
        {
            var observableGrain =
                await GetObserverGrain<IOracleObserverKasEccComponentCompleteDeferredCaseGrain, KasEccComponentDeferredResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }
    }
}
