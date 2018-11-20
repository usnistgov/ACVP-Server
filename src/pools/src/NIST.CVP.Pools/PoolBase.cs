using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using NIST.CVP.Pools.Models;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Pools
{
    public abstract class PoolBase<TParam, TResult> : IPool<TParam, TResult>
        where TParam : IParameters
        where TResult : IResult
    {
        public PoolTypes DeclaredType { get; }
        public TParam WaterType { get; }
        
        public int WaterLevel => Water.Count;
        public int MaxWaterLevel { get; }
        public decimal WaterFillPercent => (decimal)WaterLevel / MaxWaterLevel;

        public bool IsEmpty => WaterLevel == 0;

        public Type ParamType => typeof(TParam);
        public IParameters Param => WaterType;
        public Type ResultType => typeof(TResult);

        protected readonly IOracle Oracle;
        protected readonly ConcurrentQueue<ResultWrapper<TResult>> Water;
        
        private readonly IList<JsonConverter> _jsonConverters;
        private readonly IOptions<PoolConfig> _poolConfig;
        private readonly int _maxWaterReuse;
        private readonly string _fullPoolLocation;
        
        protected PoolBase(PoolConstructionParameters<TParam> param)
        {
            _poolConfig = param.PoolConfig;
            Oracle = param.Oracle;
            DeclaredType = param.PoolProperties.PoolType.Type;
            MaxWaterLevel = param.PoolProperties.MaxCapacity;
            WaterType = param.WaterType;
            _jsonConverters = param.JsonConverters;
            _maxWaterReuse = param.PoolProperties.MaxWaterReuse;
            Water = new ConcurrentQueue<ResultWrapper<TResult>>();
            _fullPoolLocation = param.FullPoolLocation;
            LoadPoolFromFile();
        }

        public PoolResult<TResult> GetNext()
        {
            if (IsEmpty)
            {
                return new PoolResult<TResult> { PoolEmpty = true };
            }
            else
            {
                var success = Water.TryDequeue(out var wrappedResult);
                if (success)
                {
                    RecycleValueWhenOptionsAllow(wrappedResult);

                    return new PoolResult<TResult>
                    {
                        Result = wrappedResult.Result,
                        TimesValueUsed = wrappedResult.TimesValueUsed
                    };
                }
                else
                {
                    throw new Exception("Unable to get next from queue");
                }
            }
        }

        public PoolResult<IResult> GetNextUntyped()
        {
            var result = GetNext();
            return new PoolResult<IResult>()
            {
                PoolEmpty = result.PoolEmpty,
                Result = result.Result,
                TimesValueUsed = result.TimesValueUsed
            };
        }

        public bool AddWater(TResult value)
        {
            return AddWater(new ResultWrapper<TResult>()
            {
                Result = value,
                TimesValueUsed = 0,
                ValueCreated = DateTime.UtcNow
            });
        }

        public bool AddWater(IResult value)
        {
            if (value.GetType() != typeof(TResult))
            {
                throw new ArgumentException($"Expecting {nameof(value)} to be of type {typeof(TResult)}");
            }

            return AddWater((TResult)value);
        }

        private void LoadPoolFromFile()
        {
            if (File.Exists(_fullPoolLocation))
            {
                // Load file
                var poolContents = JsonConvert.DeserializeObject<ResultWrapper<TResult>[]>(
                    File.ReadAllText(_fullPoolLocation),
                    new JsonSerializerSettings
                    {
                        Converters = _jsonConverters
                    }
                );

                // Pool is empty
                if (poolContents == null)
                {
                    return;
                }

                foreach (var result in poolContents)
                {
                    AddWater(result);
                }
            }
            else
            {
                // Create empty file
                File.WriteAllText(_fullPoolLocation, "[]");
                LogManager.GetCurrentClassLogger().Debug($"{_fullPoolLocation} created");
            }
        }

        public bool SavePoolToFile()
        {
            var poolContents = JsonConvert.SerializeObject(
                Water,
                new JsonSerializerSettings
                {
                    Converters = _jsonConverters,
                    Formatting = Formatting.Indented
                }
            );

            try
            {
                File.WriteAllText(_fullPoolLocation, poolContents);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CleanPool()
        {
            Water.Clear();
            return true;
        }

        private void RecycleValueWhenOptionsAllow(ResultWrapper<TResult> result)
        {
            // Recycle the water when option is configured, and TimesValueReused is less than the max reuse
            if (_poolConfig.Value.ShouldRecyclePoolWater && result.TimesValueUsed < _maxWaterReuse)
            {
                var newResultToQueue = new ResultWrapper<TResult>()
                {
                    Result = result.Result,
                    TimesValueUsed = result.TimesValueUsed + 1,
                    ValueCreated = result.ValueCreated,
                    ValueUsed = DateTime.UtcNow
                };

                Shuffle();
                AddWater(newResultToQueue);
            }
        }

        private void Shuffle()
        {
            var shuffledList = Water.OrderBy(x => Guid.NewGuid()).ToList();
            Water.Clear();
            shuffledList.ForEach(fe => Water.Enqueue(fe));
        }

        private bool AddWater(ResultWrapper<TResult> result)
        {
            Water.Enqueue(result);
            return true;
        }

        public abstract Task RequestWater();
    }
}
