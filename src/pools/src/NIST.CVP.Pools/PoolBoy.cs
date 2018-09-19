using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using System;
using System.IO;
using System.Net;
using PoolConfig = NIST.CVP.Common.Config.PoolConfig;

namespace NIST.CVP.Pools
{
    public class PoolBoy<T>
        where T : IResult
    {
        private readonly PoolConfig _poolConfig;

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

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(
                    JsonConvert.SerializeObject(paramHolder)
                );
            }

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var json = streamReader.ReadToEnd();
                    var poolResult = JsonConvert.DeserializeObject<PoolResult<T>>(json);
                    if (poolResult.PoolEmpty)
                    {
                        throw new Exception("Pool empty, defaulting to normal procedure");
                    }

                    return poolResult.Result;
                }
            }
            catch (Exception)
            {
                // Fall back to normal procedure
                return default(T);
            }
        }
    }
}
