using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Pools;
using System.Collections.Generic;

namespace NIST.CVP.PoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoolsController : Controller
    {
        private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter>
            {
                new BitstringConverter(),
                new DomainConverter(),
                new BigIntegerConverter()
            }
        };

        [HttpPost]
        // https://localhost:5001/api/pools
        public string GetDataFromPool(ParameterHolder parameterHolder)
        {
            return JsonConvert.SerializeObject(Startup.PoolManager.GetResultFromPool(parameterHolder), _jsonSettings);
        }

        [HttpPost]
        [Route("add")]
        // https://localhost:5001/api/pools/add
        public bool PostDataToPool(ParameterHolder parameterHolder)
        {
            return Startup.PoolManager.AddResultToPool(parameterHolder);
        }

        [HttpPost]
        [Route("status")]
        // https://localhost:5001/api/pools/status
        public string PoolStatus(ParameterHolder parameterHolder)
        {
            return JsonConvert.SerializeObject(Startup.PoolManager.GetPoolStatus(parameterHolder));
        }

        [HttpGet]
        [Route("save")]
        // https://localhost:5001/api/pools/save
        public bool SavePools()
        {
            return Startup.PoolManager.SavePools();
        }
    }
}
