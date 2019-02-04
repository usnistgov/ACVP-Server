using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Pools.Enums;
using NIST.CVP.Pools.Models;
using NIST.CVP.Pools.PoolModels;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Pools
{
    public class PoolManager
    {
        public readonly List<IPool> Pools = new List<IPool>();
        private readonly IOptions<PoolConfig> _poolConfig;
        private readonly IOracle _oracle;
        private readonly string _poolDirectory;
        private readonly string _poolConfigFile;

        private PoolProperties[] _properties;
        private bool _poolsLoaded;
        
        private readonly IList<JsonConverter> _jsonConverters = new List<JsonConverter>
        {
            new BitstringConverter(),
            new DomainConverter(),
            new BigIntegerConverter(),
            new StringEnumConverter()
        };

        public PoolManager(
            IOptions<PoolConfig> poolConfig, 
            IOracle oracle
        )
        {
            _poolConfig = poolConfig;
            _oracle = oracle;
            _poolDirectory = _poolConfig.Value.PoolDirectory;
            _poolConfigFile = _poolConfig.Value.PoolConfigFile;

            LoadPoolsAsync().FireAndForget();
        }

        public PoolInformation GetPoolStatus(ParameterHolder paramHolder)
        {
            if (!_poolsLoaded)
            {
                return new PoolInformation()
                {
                    PoolExists = false
                };
            }

            if (Pools.TryFirst(pool => pool.Param.Equals(paramHolder.Parameters), out var result))
            {
                return new PoolInformation {FillLevel = result.WaterLevel};
            }

            return new PoolInformation {PoolExists = false};
        }

        public bool AddResultToPool(ParameterHolder paramHolder)
        {
            if (!_poolsLoaded)
            {
                return false;
            }

            if (Pools.TryFirst(pool => pool.Param.Equals(paramHolder.Parameters), out var result))
            {
                return result.AddWater(paramHolder.Result);
            }

            return false;
        }

        public PoolResult<IResult> GetResultFromPool(ParameterHolder paramHolder)
        {
            if (!_poolsLoaded)
            {
                return new PoolResult<IResult> { PoolTooEmpty = true };
            }

            if (Pools.TryFirst(pool => pool.Param.Equals(paramHolder.Parameters), out var result))
            {
                return result.GetNextUntyped();
            }

            return new PoolResult<IResult> { PoolTooEmpty = true };
        }

        public List<ParameterHolder> GetPoolInformation()
        {
            var list = new List<ParameterHolder>();

            if (!_poolsLoaded)
            {
                return list;
            }

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
            if (!_poolsLoaded)
            {
                return false;
            }

            if (_properties.TryFirst(
                properties => properties.FilePath.Equals(poolProps.FilePath, StringComparison.OrdinalIgnoreCase),
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
            if (!_poolsLoaded)
            {
                return new List<PoolProperties>();
            }

            return new List<PoolProperties>(_properties);
        }

        public bool SavePools()
        {
            if (!_poolsLoaded)
            {
                return false;
            }

            foreach (var pool in Pools)
            {
                if (_properties.TryFirst(prop => pool.Param.Equals(prop.PoolType.Parameters), out var properties))
                {
                    if (!pool.SavePoolToFile())
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public bool SavePoolConfigs()
        {
            if (!_poolsLoaded)
            {
                return false;
            }

            var fullConfigFile = Path.Combine(_poolDirectory, _poolConfigFile);
            var json = JsonConvert.SerializeObject
            (
                _properties, 
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    Converters = _jsonConverters
                }
            );
            
            File.WriteAllText(fullConfigFile, json);
            
            return true;
        }

        public bool CleanPools()
        {
            if (!_poolsLoaded)
            {
                return false;
            }

            foreach (var pool in Pools)
            {
                pool.CleanPool();
            }

            return true;
        }

        public async Task<SpawnJobResponse> SpawnJobForMostShallowPool(int jobsToSpawn)
        {
            if (!_poolsLoaded)
            {
                return new SpawnJobResponse();
            }

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

                    RequestPoolWater(jobsToSpawn, tasks, minPool, json);

                    // If filling of pools occurred, wait for it to finish, then save the pools.
                    if (tasks.Count > 0)
                    {
                        await Task.WhenAll(tasks);

                        LogManager.GetCurrentClassLogger()
                            .Log(LogLevel.Info, $"Pool was filled. Proceeding to save pool: \n\n {json}");

                        minPool.SavePoolToFile();
                        return new SpawnJobResponse()
                        {
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
        
        private async Task LoadPoolsAsync()
        {
            LogManager.GetCurrentClassLogger()
                .Log(LogLevel.Info, "Loading Pools.");

            var fullConfigFile = Path.Combine(_poolDirectory, _poolConfigFile);
            _properties = JsonConvert.DeserializeObject<PoolProperties[]>
            (
                await File.ReadAllTextAsync(fullConfigFile), 
                new JsonSerializerSettings
                {
                    Converters = _jsonConverters
                }
            );

            foreach (var poolProperty in _properties)
            {
                var fullPoolLocation = Path.Combine(_poolDirectory, poolProperty.FilePath);
                var param = poolProperty.PoolType.Parameters;

                IPool pool = null;
                switch (poolProperty.PoolType.Type)
                {
                    // Primarily for testing purposes
                    case PoolTypes.SHA:
                        pool = new ShaPool(GetConstructionParameters(param as ShaParameters, poolProperty, fullPoolLocation));
                        break;

                    // Primarily for testing purposes
                    case PoolTypes.AES:
                        pool = new AesPool(GetConstructionParameters(param as AesParameters, poolProperty, fullPoolLocation));
                        break;

                    case PoolTypes.SHA_MCT:
                        pool = new ShaMctPool(GetConstructionParameters(param as ShaParameters, poolProperty, fullPoolLocation));
                        break;

                    case PoolTypes.AES_MCT:
                        pool = new AesMctPool(GetConstructionParameters(param as AesParameters, poolProperty, fullPoolLocation));
                        break;

                    case PoolTypes.TDES_MCT:
                        pool = new TdesMctPool(GetConstructionParameters(param as TdesParameters, poolProperty, fullPoolLocation));
                        break;

                    case PoolTypes.SHA3_MCT:
                        pool = new Sha3MctPool(GetConstructionParameters(param as Sha3Parameters, poolProperty, fullPoolLocation));
                        break;

                    case PoolTypes.CSHAKE_MCT:
                        pool = new CShakeMctPool(GetConstructionParameters(param as CShakeParameters, poolProperty, fullPoolLocation));
                        break;

                    case PoolTypes.PARALLEL_HASH_MCT:
                        pool = new ParallelHashMctPool(GetConstructionParameters(param as ParallelHashParameters, poolProperty, fullPoolLocation));
                        break;

                    case PoolTypes.TUPLE_HASH_MCT:
                        pool = new TupleHashMctPool(GetConstructionParameters(param as TupleHashParameters, poolProperty, fullPoolLocation));
                        break;

                    case PoolTypes.DSA_PQG:
                        pool = new DsaPqgPool(GetConstructionParameters(param as DsaDomainParametersParameters, poolProperty, fullPoolLocation));
                        break;

                    case PoolTypes.ECDSA_KEY:
                        pool = new EcdsaKeyPool(GetConstructionParameters(param as EcdsaKeyParameters, poolProperty, fullPoolLocation));
                        break;

                    case PoolTypes.RSA_KEY:
                        pool = new RsaKeyPool(GetConstructionParameters(param as RsaKeyParameters, poolProperty, fullPoolLocation));
                        break;

                    default:
                        throw new Exception("No pool model found");
                }

                await pool.LoadPoolFromFile();

                Pools.Add(pool);
            }

            LogManager.GetCurrentClassLogger()
                .Log(LogLevel.Info, "Pools loaded.");

            _poolsLoaded = true;
        }

        private PoolConstructionParameters<TParam> GetConstructionParameters<TParam>(TParam param, PoolProperties poolProperties, string fullPoolLocation)
            where TParam : IParameters
        {
            return new PoolConstructionParameters<TParam>()
            {
                Oracle = _oracle,
                JsonConverters = _jsonConverters,
                PoolConfig = _poolConfig,
                PoolProperties = poolProperties,
                WaterType = param,
                FullPoolLocation = fullPoolLocation
            };
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
                    LogManager.GetCurrentClassLogger()
                        .Log(LogLevel.Info, $"Starting job to fill pool with {numberOfJobsToQueue} precomputed values, using parameters: \n\n {json}");
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
