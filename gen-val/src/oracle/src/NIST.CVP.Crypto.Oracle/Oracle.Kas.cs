using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Kas;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Br2;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Br2;
using NIST.CVP.Orleans.Grains.Interfaces.Kas.Sp800_56Ar1;
using NIST.CVP.Orleans.Grains.Interfaces.Kas.Sp800_56Br2;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public Task<KasValResult> GetKasValTestAsync(KasValParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<KasAftResult> GetKasAftTestAsync(KasAftParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParameters param)
        {
            throw new System.NotImplementedException();
        }

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

            var keyTasks = new List<Task<RsaKeyResult>>();
            Task<RsaKeyResult> serverKeyTask = null;
            KeyPair serverKey = null;
            if (serverRequirements.GeneratesEphemeralKeyPair)
            {
                serverKeyTask = GetRsaKeyAsync(new RsaKeyParameters()
                {
                    Standard = Fips186Standard.Fips186_5,
                    Modulus = param.Modulo,
                    KeyFormat = param.PrivateKeyMode,
                    KeyMode = PrimeGenModes.RandomProbablePrimes,
                    PrimeTest = PrimeTestModes.TwoPow100ErrorBound,
                    PublicExponentMode = param.PublicExponentMode,
                    PublicExponent = param.PublicExponent == 0 ? null : new BitString(param.PublicExponent)
                });
                keyTasks.Add(serverKeyTask);
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
                    Standard = Fips186Standard.Fips186_5,
                    Modulus = param.Modulo,
                    KeyFormat = param.PrivateKeyMode,
                    KeyMode = PrimeGenModes.RandomProbablePrimes,
                    PrimeTest = PrimeTestModes.TwoPow100ErrorBound,
                    PublicExponentMode = param.PublicExponentMode,
                    PublicExponent = param.PublicExponent == 0 ? null : new BitString(param.PublicExponent)
                });
                keyTasks.Add(iutKeyTask);
            }

            await Task.WhenAll(keyTasks);

            if (serverKeyTask != null)
            {
                serverKey = serverKeyTask.Result.Key;
            }

            if (iutKeyTask != null)
            {
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
            // Get RSA keys on behalf of the server and IUT.
            var serverRequirements = KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                param.Scheme, param.KasMode, param.ServerKeyAgreementRole, param.ServerKeyConfirmationRole,
                param.KeyConfirmationDirection);

            var keyTasks = new List<Task<RsaKeyResult>>();
            Task<RsaKeyResult> serverKeyTask = null;
            KeyPair serverKey = null;
            if (serverRequirements.GeneratesEphemeralKeyPair)
            {
                serverKeyTask = GetRsaKeyAsync(new RsaKeyParameters()
                {
                    Standard = Fips186Standard.Fips186_5,
                    Modulus = param.Modulo,
                    KeyFormat = param.PrivateKeyMode,
                    KeyMode = PrimeGenModes.RandomProbablePrimes,
                    PrimeTest = PrimeTestModes.TwoPow100ErrorBound,
                    PublicExponentMode = param.PublicExponentMode,
                    PublicExponent = param.PublicExponent == 0 ? null : new BitString(param.PublicExponent)
                });
                keyTasks.Add(serverKeyTask);
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
                    Standard = Fips186Standard.Fips186_5,
                    Modulus = param.Modulo,
                    KeyFormat = param.PrivateKeyMode,
                    KeyMode = PrimeGenModes.RandomProbablePrimes,
                    PrimeTest = PrimeTestModes.TwoPow100ErrorBound,
                    PublicExponentMode = param.PublicExponentMode,
                    PublicExponent = param.PublicExponent == 0 ? null : new BitString(param.PublicExponent)
                });
                keyTasks.Add(iutKeyTask);
            }

            await Task.WhenAll(keyTasks);

            if (serverKeyTask != null)
            {
                serverKey = serverKeyTask.Result.Key;
            }

            if (iutKeyTask != null)
            {
                iutKey = iutKeyTask.Result.Key;
            }

            var observableGrain =
                await GetObserverGrain<IOracleObserverKasAftIfcCaseGrain, KasAftResultIfc>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, serverKey, iutKey, LoadSheddingRetries);

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
