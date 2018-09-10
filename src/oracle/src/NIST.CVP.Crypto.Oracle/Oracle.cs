using Newtonsoft.Json;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Math;
using NIST.CVP.Pools;
using NIST.CVP.Pools.Enums;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle : IOracle
    {
        private readonly Random800_90 _rand = new Random800_90();

        private const double GCM_FAIL_RATIO = .25;
        private const double XPN_FAIL_RATIO = .25;
        private const double CMAC_FAIL_RATIO = .25;
        private const double KEYWRAP_FAIL_RATIO = .2;
        private const double KMAC_FAIL_RATIO = .5;
        private const int RSA_PUBLIC_EXPONENT_BITS_MIN = 32;
        private const int RSA_PUBLIC_EXPONENT_BITS_MAX = 64;

        // TODO configurable concurrency through config file and/or GenValAppRunner parameter
        // Task scheduler should be an interim step until orleans is implemented, at which
        // point it will be managing tasks.  Currently, without limiting the tasks enqueued,
        // you can potentially cap out memory usage w/o being able to complete tasks.
        private static readonly LimitedConcurrencyLevelTaskScheduler _scheduler;
        private static readonly TaskFactory _taskFactory;

        static Oracle()
        {
            _scheduler = new LimitedConcurrencyLevelTaskScheduler(3);
            _taskFactory = new TaskFactory(_scheduler);
        }

        private IResult GetObjectFromPool(IParameters param, PoolTypes type)
        {
            //var request = (HttpWebRequest)WebRequest.Create("http://admin.dev.acvts.nist.gov:5002/api");
            var request = (HttpWebRequest)WebRequest.Create("http://localhost:5001/api/pools");
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
                    var poolResult = JsonConvert.DeserializeObject<PoolResult<MctResult<HashResult>>>(streamReader.ReadToEnd());
                    if (poolResult.PoolEmpty)
                    {
                        throw new Exception("Pool empty, defaulting to normal procedure");
                    }

                    return poolResult.Result;
                }
            }
            catch (Exception ex)
            {
                // Fall back to normal procedure
                return null;
            }
        }
    }
}
