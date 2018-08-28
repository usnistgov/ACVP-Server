using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Pools;
using NLog;
using System;
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
        // /api/pools
        public string GetDataFromPool(ParameterHolder parameterHolder)
        {
            try
            {
                return JsonConvert.SerializeObject(Program.PoolManager.GetResultFromPool(parameterHolder), _jsonSettings);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }

            return "";
        }

        [HttpPost]
        [Route("add")]
        // /api/pools/add
        public bool PostDataToPool(ParameterHolder parameterHolder)
        {
            return Program.PoolManager.AddResultToPool(parameterHolder);
        }

        [HttpPost]
        [Route("status")]
        // /api/pools/status
        public string PoolStatus(ParameterHolder parameterHolder)
        {
            return JsonConvert.SerializeObject(Program.PoolManager.GetPoolStatus(parameterHolder));
        }

        [HttpGet]
        [Route("save")]
        // /api/pools/save
        public bool SavePools()
        {
            return Program.PoolManager.SavePools();
        }
    }
}
