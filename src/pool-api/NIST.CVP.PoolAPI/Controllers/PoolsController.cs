using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Pools;
using System.Collections.Generic;

namespace NIST.CVP.PoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoolsController : Controller
    {
        private static readonly PoolManager PoolManager = new PoolManager(@"D:\ACVP\gen-val\src\pool-api\NIST.CVP.PoolAPI\Pools\testConfig.json");
        //private static readonly PoolManager PoolManager = new PoolManager(@"C:\Users\ctc\Documents\ACVP\gen-val\src\pool-api\NIST.CVP.PoolAPI\Pools\testConfig.json");

        private readonly IList<JsonConverter> _jsonConverters = new List<JsonConverter>
        {
            new BitstringConverter(),
            new DomainConverter(),
            new BigIntegerConverter()
        };

        [HttpGet]
        // https://localhost:5001/api/pools
        public string Get()
        {
            // TODO this is not a good way of doing this
            var param = new AesParameters
            {
                KeyLength = 128,
                Mode = BlockCipherModesOfOperation.Ecb,
                Direction = "encrypt",
                DataLength = 128
            };

            var json = JsonConvert.SerializeObject(
                PoolManager.GetResultFromPool(param),
                new JsonSerializerSettings
                {
                    Converters = _jsonConverters
                }
            );

            return json;
        }

        [Route("status")]
        // https://localhost:5001/api/pools/status
        public int Status()
        {
            var param = new AesParameters
            {
                KeyLength = 128,
                Mode = BlockCipherModesOfOperation.Ecb,
                Direction = "encrypt",
                DataLength = 128
            };

            return PoolManager.GetPoolCount(param);
        }
    }
}
