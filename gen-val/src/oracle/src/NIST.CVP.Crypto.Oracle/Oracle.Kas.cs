using System;
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
using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Br2;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Br2;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS.SafePrimes.Enums;
using NIST.CVP.Orleans.Grains.Interfaces.Exceptions;
using NIST.CVP.Orleans.Grains.Interfaces.Kas.Sp800_56Ar1;
using NIST.CVP.Orleans.Grains.Interfaces.Kas.Sp800_56Ar3;
using NIST.CVP.Orleans.Grains.Interfaces.Kas.Sp800_56Br2;
using NIST.CVP.Orleans.Grains.Interfaces.SafePrimes;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<FfcDomainParametersResult> GetSafePrimeGroupsDomainParameterAsync(SafePrimeParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IObserverSafePrimesGroupDomainParameterGrain, FfcDomainParametersResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);
            
                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetSafePrimeGroupsDomainParameterAsync(param);
            }
        }

        public async Task<KasValResult> GetKasValTestAsync(KasValParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IObserverKasValGrain, KasValResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKasValTestAsync(param);
            }
        }

        public async Task<KasAftResult> GetKasAftTestAsync(KasAftParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IObserverKasAftGrain, KasAftResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKasAftTestAsync(param);
            }
        }

        public async Task<NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar3.KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IObserverKasCompleteDeferredAftGrain, NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar3.KasAftDeferredResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredKasTestAsync(param);
            }
        }

        public async Task<KasValResultEcc> GetKasValTestEccAsync(KasValParametersEcc param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverKasValEccCaseGrain, KasValResultEcc>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKasValTestEccAsync(param);
            }
        }

        public async Task<KasAftResultEcc> GetKasAftTestEccAsync(KasAftParametersEcc param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverKasAftEccCaseGrain, KasAftResultEcc>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKasAftTestEccAsync(param);
            }
        }

        public async Task<NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1.KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersEcc param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverKasCompleteDeferredAftEccCaseGrain, NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1.KasAftDeferredResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredKasTestAsync(param);
            }
        }

        public async Task<KasValResultFfc> GetKasValTestFfcAsync(KasValParametersFfc param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverKasValFfcCaseGrain, KasValResultFfc>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKasValTestFfcAsync(param);
            }
        }

        public async Task<KasAftResultFfc> GetKasAftTestFfcAsync(KasAftParametersFfc param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverKasAftFfcCaseGrain, KasAftResultFfc>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKasAftTestFfcAsync(param);
            }
        }

        public async Task<NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1.KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersFfc param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverKasCompleteDeferredAftFfcCaseGrain, NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1.KasAftDeferredResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredKasTestAsync(param);
            }
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
                    PublicExponent = param.PublicExponentMode == PublicExponentModes.Fixed ? new BitString(param.PublicExponent) : null 
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
                    PublicExponent = param.PublicExponentMode == PublicExponentModes.Fixed ? new BitString(param.PublicExponent) : null
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

            return await GetKasValTestIfcAsync(param, serverKey, iutKey);
        }

        private async Task<KasValResultIfc> GetKasValTestIfcAsync(KasValParametersIfc param, KeyPair serverKey, KeyPair iutKey)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverKasValIfcCaseGrain, KasValResultIfc>();
                await GrainInvokeRetryWrapper.WrapGrainCall(
                    observableGrain.Grain.BeginWorkAsync,
                    param, serverKey, iutKey, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKasValTestIfcAsync(param, serverKey, iutKey);
            }
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
                    PublicExponent = param.PublicExponentMode == PublicExponentModes.Fixed ? new BitString(param.PublicExponent) : null
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
                    PublicExponent = param.PublicExponentMode == PublicExponentModes.Fixed ? new BitString(param.PublicExponent) : null
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

            return await GetKasAftTestIfcAsync(param, serverKey, iutKey);
        }

        private async Task<KasAftResultIfc> GetKasAftTestIfcAsync(KasAftParametersIfc param, KeyPair serverKey, KeyPair iutKey)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverKasAftIfcCaseGrain, KasAftResultIfc>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, serverKey, iutKey, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKasAftTestIfcAsync(param, serverKey, iutKey);
            }
        }

        public async Task<NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Br2.KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersIfc param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverKasCompleteDeferredAftIfcCaseGrain, NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Br2.KasAftDeferredResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredKasTestAsync(param);
            }
        }

        public async Task<KasEccComponentResult> GetKasEccComponentTestAsync(KasEccComponentParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverKasEccComponentCaseGrain, KasEccComponentResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKasEccComponentTestAsync(param);
            }
        }

        public async Task<KasEccComponentDeferredResult> CompleteDeferredKasComponentTestAsync(KasEccComponentDeferredParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverKasEccComponentCompleteDeferredCaseGrain, KasEccComponentDeferredResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredKasComponentTestAsync(param);
            }
        }

        public async Task<SafePrimesKeyVerResult> GetSafePrimesKeyVerTestAsync(SafePrimesKeyVerParameters param)
        {
            try
            {
                var observableGrain = await GetObserverGrain<IObserverSafePrimesKeyVerGrain, SafePrimesKeyVerResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetSafePrimesKeyVerTestAsync(param);
            }
        }

        public Task<KasSscAftResult> GetKasSscAftTestAsync(KasSscAftParameters param)
        {
            throw new NotImplementedException();
        }
        
        public Task<KasSscAftDeferredResult> CompleteDeferredKasSscAftTestAsync(KasSscAftDeferredParameters param)
        {
            throw new NotImplementedException();
        }
        
        public Task<KasSscValResult> GetKasSscValTestAsync(KasSscValParameters param)
        {
            throw new NotImplementedException();
        }
    }
}
