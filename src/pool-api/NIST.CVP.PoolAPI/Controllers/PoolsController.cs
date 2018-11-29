using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Pools;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                new BigIntegerConverter(),
                new StringEnumConverter()
            },
            Formatting = Formatting.Indented
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

        [HttpGet]
        // /api/pools
        public string GetDataAboutPools()
        {
            try
            {
                return JsonConvert.SerializeObject(Program.PoolManager.GetPoolInformation(), _jsonSettings);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }

            return "";
        }

        [HttpGet]
        [Route("config")]
        // /api/pools/config
        public string GetPoolConfig()
        {
            try
            {
                return JsonConvert.SerializeObject(Program.PoolManager.GetPoolProperties(), _jsonSettings);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }

            return "";
        }

        [HttpPost]
        [Route("config")]
        // /api/pools/config
        public string PostPoolConfig(PoolProperties poolProps)
        {
            try
            {
                return JsonConvert.SerializeObject(Program.PoolManager.EditPoolProperties(poolProps), _jsonSettings);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }

            return "";
        }

        [HttpPost]
        [Route("config/save")]
        // /api/pools/config/save
        public bool SavePoolConfig()
        {
            Program.PoolManager.SavePoolConfigs();

            return true;
        }

        [HttpPost]
        [Route("spawn")]
        public async Task<bool> SpawnJobForMostShallowPool([FromBody] int jobsToSpawn)
        {
            return await Program.PoolManager.SpawnJobForMostShallowPool(jobsToSpawn);
        }

        [HttpPost]
        [Route("add")]
        // /api/pools/add
        public bool PostDataToPool(ParameterHolder parameterHolder)
        {
            try
            {
                return Program.PoolManager.AddResultToPool(parameterHolder);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                return false;
            }
        }

        [HttpPost]
        [Route("status")]
        // /api/pools/status
        public string PoolStatus(ParameterHolder parameterHolder)
        {
            try
            {
                return JsonConvert.SerializeObject(Program.PoolManager.GetPoolStatus(parameterHolder));
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                return "";
            }
        }

        [HttpGet]
        [Route("save")]
        // /api/pools/save
        public bool SavePools()
        {
            return Program.PoolManager.SavePools();
        }

        [HttpPost]
        [Route("clean")]
        // /api/pools/clean
        public bool CleanPools()
        {
            // TODO should this clean THEN save? Or leave that up to the consumer?
            return Program.PoolManager.CleanPools();
        }
    }
}
