using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NIST.CVP.Pools;
using NIST.CVP.Pools.Interfaces;
using NIST.CVP.Pools.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace NIST.CVP.PoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoolsController : Controller
    {
        private readonly ILogger<PoolsController> _logger;
        private readonly JsonSerializerSettings _jsonSettings;
        private readonly PoolManager _poolManager;
        
        private readonly JsonResult _exceptionOnJsonEndpoint = new JsonResult(string.Empty)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        private bool _isFillingPool;
        
        public PoolsController(ILogger<PoolsController> logger, PoolManager poolManager, IJsonConverterProvider jsonConverterProvider)
        {
            _logger = logger;
            _poolManager = poolManager;
            _jsonSettings = new JsonSerializerSettings
            {
                Converters = jsonConverterProvider.GetJsonConverters(),
                Formatting = Formatting.Indented
            };
        }

        [HttpPost]
        [Produces("application/json")]
        // /api/pools
        public async Task<JsonResult> GetDataFromPool(ParameterHolder parameterHolder)
        {
            try
            {
                return new JsonResult(await _poolManager.GetResultFromPool(parameterHolder), _jsonSettings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.StackTrace);
                return _exceptionOnJsonEndpoint;
            }
        }

        [HttpGet]
        [Produces("application/json")]
        // /api/pools
        public JsonResult GetDataAboutPools()
        {
            try
            {
                return new JsonResult(_poolManager.GetPoolInformation(), _jsonSettings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.StackTrace);
                return _exceptionOnJsonEndpoint;
            }
        }

        [HttpGet]
        [Route("config")]
        [Produces("application/json")]
        // /api/pools/config
        public JsonResult GetPoolConfig()
        {
            try
            {
                return new JsonResult(_poolManager.GetPoolProperties(), _jsonSettings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.StackTrace);
                return _exceptionOnJsonEndpoint;
            }
        }

        [HttpPost]
        [Route("config")]
        // /api/pools/config
        public JsonResult PostPoolConfig(PoolProperties poolProps)
        {
            try
            {
                return new JsonResult(_poolManager.EditPoolProperties(poolProps), _jsonSettings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.StackTrace);
                return _exceptionOnJsonEndpoint;
            }
        }

        [HttpPost]
        [Route("config/save")]
        // /api/pools/config/save
        public bool SavePoolConfig()
        {
            _poolManager.SavePoolConfigs();
            return true;
        }

        [HttpPost]
        [Route("spawn")]
        public async Task<bool> SpawnJobForMostShallowPool([FromBody] int jobsToSpawn)
        {
            var result = await _poolManager.SpawnJobForMostShallowPool(jobsToSpawn);

            return result.HasSpawnedJob;
        }

        [HttpPost]
        [Route("spawn/all")]
        // /api/pools/spawn
        public async Task<bool> SpawnJobForPools([FromBody] int numberOfJobsPerPoolToQueue = 1)
        {
            // Already in process of filling pool
            if (_isFillingPool)
            {
                return false;
            }

            try
            {
                _isFillingPool = true;
                List<Task> tasks = new List<Task>();
                foreach (var pool in _poolManager.Pools)
                {
                    RequestPoolWater(numberOfJobsPerPoolToQueue, tasks, pool);
                }

                // If filling of pools occurred, wait for it to finish, then save the pools.
                if (tasks.Count > 0)
                {
                    await Task.WhenAll(tasks);

                    _logger.LogInformation("Pools have been filled.");
                }

                _isFillingPool = false;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.StackTrace);
                return false;
            }
        }

        private static void RequestPoolWater(int numberOfJobsToQueue, List<Task> tasks, IPool pool)
        {
            // If the pool is looking a bit low
            if (pool.WaterLevel < pool.MaxWaterLevel)
            {
                // Don't queue more than the max allowed for the pool
                var potentialMaxJobs = pool.MaxWaterLevel - pool.WaterLevel;
                var jobsToQueue = numberOfJobsToQueue < potentialMaxJobs
                    ? numberOfJobsToQueue
                    : potentialMaxJobs;

                // Add jobs to a list of tasks
                for (var i = 0; i < jobsToQueue; i++)
                {
                    tasks.Add(pool.RequestWater());
                }
            }
        }

        [HttpPost]
        [Route("add")]
        // /api/pools/add
        public async Task<bool> PostDataToPool(ParameterHolder parameterHolder)
        {
            try
            {
                return await _poolManager.AddResultToPool(parameterHolder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.StackTrace);
                return false;
            }
        }

        [HttpPost]
        [Route("status")]
        // /api/pools/status
        public JsonResult PoolStatus(ParameterHolder parameterHolder)
        {
            try
            {
                return new JsonResult(_poolManager.GetPoolStatus(parameterHolder), _jsonSettings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.StackTrace);
                return new JsonResult("");
            }
        }

        [HttpPost]
        [Route("status/name")]
        // /api/pools/status/name
        public JsonResult PoolStatusNames(PoolNames names)
        {
            try
            {
                return new JsonResult(names.Names.Select(pn => _poolManager.GetPoolStatus(pn)), _jsonSettings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.StackTrace);
                return new JsonResult("");
            }
        }

        [HttpPost]
        [Route("clean")]
        // /api/pools/clean
        public async Task<bool> CleanPools()
        {
            // TODO should this clean THEN save? Or leave that up to the consumer?
            return await _poolManager.CleanPools();
        }
    }
}
