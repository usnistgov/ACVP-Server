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
using System.IO;
using System.Net;
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

        public PoolBoy(IOptions<PoolConfig> poolConfig)
        {
            if (poolConfig != null)
            {
                _poolConfig = poolConfig.Value;
            }
        }

        public T GetObjectFromPool(IParameters param, PoolTypes type)
        {
            if (_poolConfig == null)
            {
                return default(T);
            }

            var request = (HttpWebRequest)WebRequest.Create($"{_poolConfig.RootUrl}:{_poolConfig.Port}/api/pools");
            request.ContentType = "application/json";
            request.Method = "POST";

            var paramHolder = new ParameterHolder
            {
                Parameters = param,
                Type = type
            };

            ThisLogger.Info($"Attempting to grab pool value for {JsonConvert.SerializeObject(paramHolder)}");

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(
                    JsonConvert.SerializeObject(
                        paramHolder,
                        new JsonSerializerSettings()
                        {
                            Converters = _jsonConverters
                        }
                    )
                );
            }

            try
            {
                var response = (HttpWebResponse) request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var json = streamReader.ReadToEnd();
                    var poolResult = JsonConvert.DeserializeObject<PoolResult<T>>(
                        json, new JsonSerializerSettings()
                        {
                            Converters = _jsonConverters
                        }
                    );
                    if (poolResult.PoolTooEmpty)
                    {
                        return default(T);
                    }

                    if (_poolConfig.ShouldLogPoolValueUse)
                    {
                        if (_poolConfig.PoolResultLogLength == 0 || json.Length <= _poolConfig.PoolResultLogLength)
                        {
                            ThisLogger.Info($"Using pool value: {json}");
                        }
                        else
                        {
                            ThisLogger.Info($"Using pool value: {json.Substring(0, _poolConfig.PoolResultLogLength)}");
                        }

                    }

                    return poolResult.Result;
                }
            }
            catch (Exception ex)
            {
                // Fall back to normal procedure
                ThisLogger.Error(ex, ex.StackTrace);
                return default(T);
            }
        }
    }
}
