using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Math.JsonConverters;
using NIST.CVP.Pools.Enums;
using NIST.CVP.Pools.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using PoolConfig = NIST.CVP.Common.Config.PoolConfig;

namespace NIST.CVP.Pools
{
    public class PoolBoy<T>
        where T : IResult
    {
        private readonly ILogger ThisLogger = LogManager.GetCurrentClassLogger();

        private readonly PoolConfig _poolConfig;
        private readonly IList<JsonConverter> _jsonConverters = new List<JsonConverter>
        {
            new BitstringBitLengthConverter(),
            new DomainConverter(),
            new BigIntegerConverter(),
            new StringEnumConverter()
        };
        private readonly HttpClient _httpClient;

        public PoolBoy(IOptions<PoolConfig> poolConfig, IHttpClientFactory httpClientFactory)
        {
            if (poolConfig != null)
            {
                _poolConfig = poolConfig.Value;
                _httpClient = httpClientFactory.CreateClient(GetType().FullName);
                _httpClient.BaseAddress = new UriBuilder()
                {
                    Host = _poolConfig.RootUrl,
                    Path = "/api/pools",
                    Port = _poolConfig.Port
                }.Uri;
                _httpClient.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        public async Task<T> GetObjectFromPoolAsync(IParameters param, PoolTypes type)
        {
            if (_poolConfig == null)
            {
                return default;
            }

            var paramHolder = new ParameterHolder
            {
                Parameters = param,
                Type = type
            };

            ThisLogger.Debug($"Attempting to grab pool value for {JsonConvert.SerializeObject(paramHolder)}");
            var paramHolderJson = JsonConvert.SerializeObject(
                paramHolder,
                new JsonSerializerSettings()
                {
                    Converters = _jsonConverters
                }
            );

            try
            {
                var response = await _httpClient.PostAsync(
                    _httpClient.BaseAddress.AbsoluteUri, new StringContent(paramHolderJson, Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    ThisLogger.Error($"Pool boy failure, status code {response.StatusCode} from pool parameter: {paramHolderJson}");
                    return default;
                }
            
                var resultJson = await response.Content.ReadAsStringAsync();
                var poolResult = JsonConvert.DeserializeObject<PoolResult<T>>(
                    resultJson, new JsonSerializerSettings()
                    {
                        Converters = _jsonConverters
                    }
                );
            
                if (poolResult.PoolTooEmpty)
                {
                    if (_poolConfig.ShouldLogPoolValueUse)
                    {
                        ThisLogger.Warn($"Pool too empty for pool parameter: {paramHolderJson}");
                    }
                    return default;
                }

                if (_poolConfig.ShouldLogPoolValueUse)
                {
                    if (_poolConfig.PoolResultLogLength == 0 || resultJson.Length <= _poolConfig.PoolResultLogLength)
                    {
                        ThisLogger.Debug($"Using pool value: {resultJson}");
                    }
                    else
                    {
                        ThisLogger.Debug($"Using pool value: {resultJson.Substring(0, _poolConfig.PoolResultLogLength)}");
                    }
                }
            
                return poolResult.Result;
            }
            catch (Exception ex)
            {
                // Fall back to normal procedure
                ThisLogger.Error(ex, ex.StackTrace);
                return default;
            }
        }
    }
}
