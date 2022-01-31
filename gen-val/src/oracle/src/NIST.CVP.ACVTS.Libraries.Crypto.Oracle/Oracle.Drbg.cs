using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Helpers;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Drbg;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Exceptions;
using DrbgResult = NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.DrbgResult;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<DrbgResult> GetDrbgCaseAsync(DrbgParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverDrbgCaseGrain, DrbgResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetDrbgCaseAsync(param);
            }
        }

        public async Task<HashResult> GetHashDfCaseAsync(ShaWrapperParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverHashDfCaseGrain, HashResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetHashDfCaseAsync(param);
            }
        }

        public async Task<AesResult> GetBlockCipherDfCaseAsync(AesParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverBlockCipherDfCaseGrain, AesResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetBlockCipherDfCaseAsync(param);
            }
        }
    }
}
