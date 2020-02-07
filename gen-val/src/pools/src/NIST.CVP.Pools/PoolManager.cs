using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using NIST.CVP.Pools.Interfaces;
using NIST.CVP.Pools.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NIST.CVP.Pools
{
    public class PoolManager
    {
        private ILogger<PoolManager> _logger;
        
        public readonly List<IPool> Pools = new List<IPool>();
        private readonly IOptions<PoolConfig> _poolConfig;
        private readonly string _poolConfigFile;
        private readonly IPoolLogRepository _poolLogRepository;
        private readonly IPoolFactory _poolFactory;

        private PoolProperties[] _properties;

        private readonly IList<JsonConverter> _jsonConverters;
        
        public PoolManager(
            ILogger<PoolManager> logger,
            IOptions<PoolConfig> poolConfig,
            IPoolLogRepository poolLogRepository,
            IPoolFactory poolFactory,
            IJsonConverterProvider jsonConverterProvider
        )
        {
            _logger = logger;
            _poolConfig = poolConfig;
            _poolConfigFile = _poolConfig.Value.PoolConfigFile;
            _poolLogRepository = poolLogRepository;
            _poolFactory = poolFactory;
            _jsonConverters = jsonConverterProvider.GetJsonConverters();

            LoadPools();
        }

        public PoolInformation GetPoolStatus(string poolName)
        {
            if (Pools.TryFirst(pool => pool.PoolName.Equals(poolName, StringComparison.OrdinalIgnoreCase),
                out var result))
            {
                return new PoolInformation { PoolName = result.PoolName, FillLevel = result.WaterLevel };
            }
            
            return new PoolInformation { PoolExists = false };
        }

        public PoolInformation GetPoolStatus(ParameterHolder paramHolder)
        {
            if (Pools.TryFirst(pool => pool.Param.Equals(paramHolder.Parameters), out var result))
            {
                return new PoolInformation { PoolName = result.PoolName, FillLevel = result.WaterLevel };
            }

            return new PoolInformation { PoolExists = false };
        }

        public bool AddResultToPool(ParameterHolder paramHolder)
        {
            if (Pools.TryFirst(pool => pool.Param.Equals(paramHolder.Parameters), out var result))
            {
                return result.AddWater(paramHolder.Result);
            }

            return false;
        }

        public PoolResult<IResult> GetResultFromPool(ParameterHolder paramHolder)
        {
            var startAction = DateTime.Now;

            if (Pools.TryFirst(pool => pool.Param.Equals(paramHolder.Parameters), out var result))
            {
                return result.GetNextUntyped();
            }

            _poolLogRepository.WriteLog(
                LogTypes.NoPool,
                string.Empty,
                startAction,
                DateTime.Now,
                JsonConvert.SerializeObject(paramHolder.Parameters, new JsonSerializerSettings() { Converters = _jsonConverters }));
            return new PoolResult<IResult> { PoolTooEmpty = true };
        }

        public List<ParameterHolder> GetPoolInformation()
        {
            var list = new List<ParameterHolder>();

            Pools.ForEach(fe =>
            {
                list.Add(new ParameterHolder
                {
                    Parameters = fe.Param,
                    Type = fe.DeclaredType
                });
            });

            return list;
        }

        public bool EditPoolProperties(PoolProperties poolProps)
        {
            if (_properties.TryFirst(
                properties => properties.PoolName.Equals(poolProps.PoolName, StringComparison.OrdinalIgnoreCase),
                out var result))
            {
                result.MaxCapacity = poolProps.MaxCapacity;
                result.MinCapacity = poolProps.MinCapacity;
                result.RecycleRatePerHundred = poolProps.RecycleRatePerHundred;
            }

            return true;
        }

        public List<PoolProperties> GetPoolProperties()
        {
            return new List<PoolProperties>(_properties);
        }

        public bool SavePoolConfigs()
        {
            var json = JsonConvert.SerializeObject
            (
                _properties,
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    Converters = _jsonConverters
                }
            );

            File.WriteAllText(_poolConfigFile, json);

            return true;
        }

        public bool CleanPools()
        {
            foreach (var pool in Pools)
            {
                pool.CleanPool();
            }

            return true;
        }

        public async Task<SpawnJobResponse> SpawnJobForMostShallowPool(int jobsToSpawn)
        {
            try
            {
                List<Task> tasks = new List<Task>();
                IPool minPool = GetMinimallyFilledPool();

                // If the pool is looking a bit low
                if (minPool != null)
                {
                    var json = JsonConvert.SerializeObject(
                        minPool.Param, new JsonSerializerSettings()
                        {
                            Converters = _jsonConverters,
                            Formatting = Formatting.Indented
                        }
                    );

                    var startAction = DateTime.Now;
                    RequestPoolWater(jobsToSpawn, tasks, minPool, json);

                    // If filling of pools occurred, wait for it to finish, then save the pools.
                    if (tasks.Count > 0)
                    {
                        await Task.WhenAll(tasks);

                        _poolLogRepository.WriteLog(LogTypes.QueueOrleansWorkToPool, minPool.PoolName, startAction,
                            DateTime.Now, null);

                        _logger.LogInformation($"Pool was filled: \n\n {json}");

                        return new SpawnJobResponse()
                        {
                            PoolName = minPool.PoolName,
                            HasSpawnedJob = true,
                            PoolParameter = minPool.Param
                        };
                    }
                }

                // Nothing was queued
                return new SpawnJobResponse();
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                return new SpawnJobResponse();
            }
        }

        private void LoadPools()
        {
            _logger.LogInformation("Loading Pools.");

            var configFileFound = File.Exists(_poolConfigFile);
            _logger.LogInformation($"Loading Config file: {_poolConfigFile}. File found: {configFileFound}");

            _properties = JsonConvert.DeserializeObject<PoolProperties[]>
            (
                File.ReadAllText(_poolConfigFile),
                new JsonSerializerSettings
                {
                    Converters = _jsonConverters
                }
            );

            _logger.LogInformation("Config loaded.");

            foreach (var poolProperty in _properties)
            {
                _logger.LogInformation($"Attempting to load {poolProperty.PoolName}");

                try
                {
                    Pools.Add(_poolFactory.GetPool(poolProperty));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    throw;
                }

                _logger.LogInformation($"{poolProperty.PoolName} loaded.");
            }

            _logger.LogInformation("Pools loaded.");
        }

        private IPool GetMinimallyFilledPool()
        {
            // Get a random pool with the minimum percentage
            var minPercent = Pools
                .Min(m => m.WaterFillPercent);
            var minPool = Pools
                .Where(w => w.WaterFillPercent == minPercent)
                .ToList().Shuffle().FirstOrDefault();
            return minPool;
        }

        private void RequestPoolWater(int numberOfJobsToQueue, List<Task> tasks, IPool pool, string json)
        {
            // If the pool is looking a bit low
            if (pool.WaterLevel < pool.MaxWaterLevel)
            {
                // Don't queue more than the max allowed for the pool
                var potentialMaxJobs = pool.MaxWaterLevel - pool.WaterLevel;
                var jobsToQueue = numberOfJobsToQueue < potentialMaxJobs
                    ? numberOfJobsToQueue
                    : potentialMaxJobs;

                if (jobsToQueue > 0)
                {
                    _logger.LogInformation($"Starting job to fill pool with {numberOfJobsToQueue} precomputed values, using parameters: \n\n {json}");
                }

                // Add jobs to a list of tasks
                for (var i = 0; i < jobsToQueue; i++)
                {
                    tasks.Add(pool.RequestWater());
                }
            }
        }
    }
}
