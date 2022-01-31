using System;
using System.Numerics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Helpers;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Dsa;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Exceptions;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Oracle
{
    public partial class Oracle
    {
        public virtual async Task<DsaDomainParametersResult> GetDsaPQAsync(DsaDomainParametersParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverDsaPqCaseGrain, DsaDomainParametersResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetDsaPQAsync(param);
            }
        }

        public async Task<DsaDomainParametersResult> GetDsaGAsync(DsaDomainParametersParameters param, DsaDomainParametersResult pqParam)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverDsaGCaseGrain, DsaDomainParametersResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, pqParam, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetDsaGAsync(param, pqParam);
            }
        }

        public virtual async Task<DsaDomainParametersResult> GetDsaDomainParametersAsync(DsaDomainParametersParameters param)
        {
            var pqResult = await GetDsaPQAsync(param);
            var gResult = pqResult;
            if (pqResult.G == default(BigInteger))
            {
                // Only try to get a G if the previous call didn't access the pool
                gResult = await GetDsaGAsync(param, pqResult);
            }

            return new DsaDomainParametersResult
            {
                Counter = pqResult.Counter,
                G = gResult.G,
                H = gResult.H,
                Index = gResult.Index,
                P = pqResult.P,
                Q = pqResult.Q,
                Seed = pqResult.Seed
            };
        }

        public async Task<VerifyResult<DsaDomainParametersResult>> GetDsaPQVerifyAsync(DsaDomainParametersParameters param, DsaDomainParametersResult fullParam)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverDsaPqVerifyCaseGrain, VerifyResult<DsaDomainParametersResult>>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetDsaPQVerifyAsync(param, fullParam);
            }
        }

        public async Task<VerifyResult<DsaDomainParametersResult>> GetDsaGVerifyAsync(DsaDomainParametersParameters param, DsaDomainParametersResult fullParam)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverDsaGVerifyCaseGrain, VerifyResult<DsaDomainParametersResult>>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetDsaPQVerifyAsync(param, fullParam);
            }
        }

        public async Task<DsaKeyResult> GetDsaKeyAsync(DsaKeyParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverDsaKeyCaseGrain, DsaKeyResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetDsaKeyAsync(param);
            }
        }

        public async Task<VerifyResult<DsaKeyResult>> CompleteDeferredDsaKeyAsync(DsaKeyParameters param, DsaKeyResult fullParam)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverDsaCompleteDeferredKeyCaseGrain, VerifyResult<DsaKeyResult>>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredDsaKeyAsync(param, fullParam);
            }
        }

        public async Task<DsaSignatureResult> GetDeferredDsaSignatureAsync(DsaSignatureParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverDsaDeferredSignatureCaseGrain, DsaSignatureResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetDeferredDsaSignatureAsync(param);
            }
        }

        public async Task<VerifyResult<DsaSignatureResult>> CompleteDeferredDsaSignatureAsync(DsaSignatureParameters param, DsaSignatureResult fullParam)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverDsaCompleteDeferredSignatureCaseGrain, VerifyResult<DsaSignatureResult>>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredDsaSignatureAsync(param, fullParam);
            }
        }

        public async Task<DsaSignatureResult> GetDsaSignatureAsync(DsaSignatureParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverDsaSignatureCaseGrain, DsaSignatureResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetDsaSignatureAsync(param);
            }
        }
    }
}
