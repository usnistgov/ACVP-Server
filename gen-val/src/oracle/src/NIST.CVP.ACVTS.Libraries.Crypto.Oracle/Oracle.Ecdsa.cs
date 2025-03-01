﻿using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Helpers;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Ecdsa;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Exceptions;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<EccDomainParametersResult> GetEcdsaDomainParameterAsync(EcdsaCurveParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IObserverEcdsaDomainParameterGrain, EccDomainParametersResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetEcdsaDomainParameterAsync(param);
            }
        }

        public virtual async Task<EcdsaKeyResult> GetEcdsaKeyAsync(EcdsaKeyParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverEcdsaKeyCaseGrain, EcdsaKeyResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                var result = await observableGrain.ObserveUntilResult();

                if (param.Disposition != EcdsaKeyDisposition.None)
                {
                    var alterParams = new EcdsaAlterKeyParameters
                    {
                        Curve = param.Curve, Disposition = param.Disposition, Key = result.Key
                    };
                    
                    return await GetEcdsaAlterKeyAsync(alterParams);
                }
                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetEcdsaKeyAsync(param);
            }
        }

        public async Task<EcdsaKeyResult> CompleteDeferredEcdsaKeyAsync(EcdsaKeyParameters param, EcdsaKeyResult fullParam)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverEcdsaCompleteDeferredKeyCaseGrain, EcdsaKeyResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredEcdsaKeyAsync(param, fullParam);
            }
        }

        // Expects a good key to be provided as input
        public async Task<EcdsaKeyResult> GetEcdsaAlterKeyAsync(EcdsaAlterKeyParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverEcdsaAlterKeyCaseGrain, EcdsaKeyResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetEcdsaAlterKeyAsync(param);
            }
        }

        public async Task<EcdsaSignatureResult> GetDeferredEcdsaSignatureAsync(EcdsaSignatureParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverEcdsaDeferredSignatureCaseGrain, EcdsaSignatureResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetDeferredEcdsaSignatureAsync(param);
            }
        }

        public async Task<VerifyResult<EcdsaSignatureResult>> CompleteDeferredEcdsaSignatureAsync(EcdsaSignatureParameters param, EcdsaSignatureResult fullParam)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverEcdsaCompleteDeferredSignatureCaseGrain, VerifyResult<EcdsaSignatureResult>>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredEcdsaSignatureAsync(param, fullParam);
            }
        }

        public async Task<EcdsaSignatureResult> GetEcdsaSignatureAsync(EcdsaSignatureParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverEcdsaSignatureCaseGrain, EcdsaSignatureResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetEcdsaSignatureAsync(param);
            }
        }

        public async Task<VerifyResult<EcdsaSignatureResult>> GetEcdsaVerifyResultAsync(EcdsaSignatureParameters param)
        {
            var keyParam = new EcdsaKeyParameters
            {
                Curve = param.Curve
            };

            var key = await GetEcdsaKeyAsync(keyParam);
            // re-signs with "bad key" under specific error condition to ensure IUT validates as failed verification.
            EcdsaKeyResult badKey = null;
            if (param.Disposition == EcdsaSignatureDisposition.ModifyKey)
                badKey = await GetEcdsaKeyAsync(keyParam);

            return await GetEcdsaVerifyResultAsync(param, key, badKey);
        }

        private async Task<VerifyResult<EcdsaSignatureResult>> GetEcdsaVerifyResultAsync(EcdsaSignatureParameters param,
            EcdsaKeyResult key, EcdsaKeyResult badKey)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverEcdsaVerifySignatureCaseGrain, VerifyResult<EcdsaSignatureResult>>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, key, badKey, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetEcdsaVerifyResultAsync(param, key, badKey);
            }
        }
    }
}
