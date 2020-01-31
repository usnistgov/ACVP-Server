using System;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Ecdsa;
using NIST.CVP.Orleans.Grains.Interfaces.Exceptions;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<EccDomainParameters> GetEcdsaDomainParameterAsync(Curve param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IObserverEcdsaDomainParameterGrain, EccDomainParameters>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
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

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
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
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredEcdsaKeyAsync(param, fullParam);
            }
        }

        public async Task<VerifyResult<EcdsaKeyResult>> GetEcdsaKeyVerifyAsync(EcdsaKeyParameters param)
        {
            var key = await GetEcdsaKeyAsync(param);

            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverEcdsaVerifyKeyCaseGrain, VerifyResult<EcdsaKeyResult>>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, key, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetEcdsaKeyVerifyAsync(param);
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
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
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
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
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
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
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
            var badKey = await GetEcdsaKeyAsync(keyParam);

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
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetEcdsaVerifyResultAsync(param, key, badKey);
            }
        }
    }
}
