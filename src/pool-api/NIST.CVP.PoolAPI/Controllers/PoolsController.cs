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
        private readonly IList<JsonConverter> _jsonConverters = new List<JsonConverter>
        {
            new BitstringConverter(),
            new DomainConverter(),
            new BigIntegerConverter()
        };

        [HttpPost]
        // https://localhost:5001/api/pools
        public string Get(ParameterHolder parameterHolder)
        {
            var json = JsonConvert.SerializeObject(
                Startup.PoolManager.GetResultFromPool(parameterHolder.Parameters),
                new JsonSerializerSettings
                {
                    Converters = _jsonConverters
                }
            );

            return json;
        }

        [HttpPost]
        [Route("status")]
        // https://localhost:5001/api/pools/status
        public int Status(ParameterHolder parameterHolder)
        {
            return Startup.PoolManager.GetPoolCount(parameterHolder.Parameters);
        }
    }
}
