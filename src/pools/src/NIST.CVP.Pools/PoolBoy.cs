using Newtonsoft.Json;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using System;
using System.IO;
using System.Net;
using NIST.CVP.Common.Config;
using Microsoft.Extensions.Options;

namespace NIST.CVP.Pools
{
    public class PoolBoy<T>
        where T : IResult
    {
        private readonly PoolConfig _poolConfig;

        public PoolBoy(IOptions<PoolConfig> poolConfig)
        {
            _poolConfig = poolConfig.Value;
        }

        public T GetObjectFromPool(IParameters param, PoolTypes type)
        {
            var request = (HttpWebRequest)WebRequest.Create($"{_poolConfig.RootUrl}:{_poolConfig.Port}/api/pools");
            //var request = (HttpWebRequest)WebRequest.Create("http://localhost:5001/api/pools");
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
