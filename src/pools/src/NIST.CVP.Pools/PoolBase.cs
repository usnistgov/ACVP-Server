using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Math;
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
        public int MinWaterLevel { get; }
        public int MaxStagingLevel { get; }
        public decimal RecycleRate { get; }
        public decimal WaterFillPercent => (decimal)WaterLevel / MaxWaterLevel;

        public bool IsEmpty => WaterLevel == 0;

        public Type ParamType => typeof(TParam);
        public IParameters Param => WaterType;
        public Type ResultType => typeof(TResult);

        protected readonly IRandom800_90 Random;
        protected readonly IOracle Oracle;
        protected readonly ConcurrentQueue<TResult> Water;
        protected readonly ConcurrentQueue<TResult> StagingWater;
        
        private readonly IList<JsonConverter> _jsonConverters;
        private readonly IOptions<PoolConfig> _poolConfig;
        private readonly string _fullPoolLocation;
        
        protected PoolBase(PoolConstructionParameters<TParam> param)
        {
            _poolConfig = param.PoolConfig;
            _jsonConverters = param.JsonConverters;
            _fullPoolLocation = param.FullPoolLocation;

            DeclaredType = param.PoolProperties.PoolType.Type;
            MaxWaterLevel = param.PoolProperties.MaxCapacity;
            MinWaterLevel = param.PoolProperties.MinCapacity;
            MaxStagingLevel = MinWaterLevel / 10;
            RecycleRate = param.PoolProperties.RecycleRatePerHundred / 100;
            WaterType = param.WaterType;

            Oracle = param.Oracle;

            Random = new Random800_90();
            Water = new ConcurrentQueue<TResult>();
            StagingWater = new ConcurrentQueue<TResult>();
        }

        public PoolResult<TResult> GetNext()
        {
            if (WaterLevel < MinWaterLevel)
            {
                return new PoolResult<TResult> { PoolTooEmpty = true };
            }

            var success = Water.TryDequeue(out var result);
            if (success)
            {
                RecycleValueWhenOptionsAllow(result);
                return new PoolResult<TResult>
                {
                    Result = result
                };
            }

            throw new Exception("Unable to get next from queue");
        }

        public PoolResult<IResult> GetNextUntyped()
        {
            var result = GetNext();
            return new PoolResult<IResult>
            {
                PoolTooEmpty = result.PoolTooEmpty,
                Result = result.Result,
            };
        }

        public bool AddWater(TResult value)
        {
            Water.Enqueue(value);
            return true;
        }

        public bool AddWater(IResult value)
        {
            if (value.GetType() != typeof(TResult))
            {
                throw new ArgumentException($"Expecting {nameof(value)} to be of type {typeof(TResult)}");
            }

            return AddWater((TResult)value);
        }

        public bool AddWaterToStagingWater(TResult value)
        {
            StagingWater.Enqueue(value);
            return true;
        }

        public bool MixStagingIntoPool()
        {
            foreach (var value in StagingWater)
            {
                AddWater(value);
            }

            Shuffle();

            return true;
        }

        public async Task LoadPoolFromFile()
        {
            if (File.Exists(_fullPoolLocation))
            {
                // Load file
                var poolContents = JsonConvert.DeserializeObject<TResult[]>(
                    await File.ReadAllTextAsync(_fullPoolLocation),
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
            // Make sure we take all the content
            MixStagingIntoPool();

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

        private void RecycleValueWhenOptionsAllow(TResult result)
        {
            // Recycle the water when option is configured, and TimesValueReused is less than the max reuse
            if (_poolConfig.Value.ShouldRecyclePoolWater)
            {
                // Recycle rate is [0, 1)
                var probToReuse = Random.GetRandomDecimal();
                if (probToReuse < RecycleRate)
                {
                    // Recycle value
                    AddWaterToStagingWater(result);

                    // CHeck if staged water needs to be mixed in
                    if (StagingWater.Count > MaxStagingLevel)
                    {
                        MixStagingIntoPool();
                        StagingWater.Clear();
                    }
                }
            }
        }

        private void Shuffle()
        {
            var shuffledList = Water.OrderBy(x => Guid.NewGuid()).ToList();
            Water.Clear();
            shuffledList.ForEach(fe => Water.Enqueue(fe));
        }

        public abstract Task RequestWater();
    }
}
