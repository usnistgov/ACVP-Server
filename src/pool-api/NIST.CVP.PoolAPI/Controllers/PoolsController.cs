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
        private bool _isFillingPool = false;

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
        public async Task<bool> SpawnJobForMostShallowPool([FromBody] int numberOfJobsToQueue)
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

                // Get a random pool with the minimum percentage
                var minPercent = Program.PoolManager.Pools
                    .Min(m => m.WaterFillPercent);
                var minPool = Program.PoolManager.Pools
                    .Where(w => w.WaterFillPercent == minPercent)
                    .ToList().Shuffle().FirstOrDefault();

                // If the pool is looking a bit low
                if (minPool != null)
                {
                    RequestPoolWater(numberOfJobsToQueue, tasks, minPool);
                    
                    // If filling of pools occurred, wait for it to finish, then save the pools.
                    if (tasks.Count > 0)
                    {
                        await Task.WhenAll(tasks);

                        var json = JsonConvert.SerializeObject(minPool.Param, _jsonSettings);

                        LogManager.GetCurrentClassLogger()
                            .Log(LogLevel.Info, $"Pool was filled. Proceeding to save pool: \n\n {json}");
                        
                        minPool.SavePoolToFile();
                    }
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

        [HttpPost]
        [Route("spawn/all")]
        // /api/pools/spawn
        public async Task<bool> SpawnJobForPools([FromBody] int numberOfJobsPerPoolToQueue)
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
                foreach (var pool in Program.PoolManager.Pools)
                {
                    RequestPoolWater(numberOfJobsPerPoolToQueue, tasks, pool);
                }

                // If filling of pools occurred, wait for it to finish, then save the pools.
                if (tasks.Count > 0)
                {
                    await Task.WhenAll(tasks);

                    LogManager.GetCurrentClassLogger()
                        .Log(LogLevel.Info, "Pools have been filled. Proceeding to save.");

                    SavePools();
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
    }
}
