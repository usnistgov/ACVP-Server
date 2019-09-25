using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NIST.CVP.Pools;
using NIST.CVP.Pools.Interfaces;
using NIST.CVP.Pools.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace NIST.CVP.PoolAPI.Controllers
{
    [Route("api/[controller]")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [ApiController]
    public class PoolsController : Controller
    {
        private readonly PoolManager _poolManager;
        private bool _isFillingPool = false;

        private readonly JsonSerializerSettings _jsonSettings;

        public PoolsController(PoolManager poolManager, IJsonConverterProvider jsonConverterProvider)
        {
            _poolManager = poolManager;
            _jsonSettings = new JsonSerializerSettings
            {
                Converters = jsonConverterProvider.GetJsonConverters(),
                Formatting = Formatting.Indented
            };

        }

        [HttpPost]
        // /api/pools
        public string GetDataFromPool(ParameterHolder parameterHolder)
        {
            try
            {
                return JsonConvert.SerializeObject(_poolManager.GetResultFromPool(parameterHolder), _jsonSettings);
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
                return JsonConvert.SerializeObject(_poolManager.GetPoolInformation(), _jsonSettings);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }

            return "";
        }

        [HttpGet]
        [Route("config")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        // /api/pools/config
        public string GetPoolConfig()
        {
            try
            {
                return JsonConvert.SerializeObject(_poolManager.GetPoolProperties(), _jsonSettings);
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
                return JsonConvert.SerializeObject(_poolManager.EditPoolProperties(poolProps), _jsonSettings);
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

                    LogManager.GetCurrentClassLogger()
                        .Log(LogLevel.Info, "Pools have been filled.");
                }

                _isFillingPool = false;

                return true;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
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
        public bool PostDataToPool(ParameterHolder parameterHolder)
        {
            try
            {
                return _poolManager.AddResultToPool(parameterHolder);
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
                return JsonConvert.SerializeObject(_poolManager.GetPoolStatus(parameterHolder));
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                return "";
            }
        }

        [HttpPost]
        [Route("clean")]
        // /api/pools/clean
        public bool CleanPools()
        {
            // TODO should this clean THEN save? Or leave that up to the consumer?
            return _poolManager.CleanPools();
        }
    }
}
