using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Math;
using NIST.CVP.Pools.Enums;
using NIST.CVP.Pools.Interfaces;
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
        
        public string PoolName { get; }

        public long WaterLevel { get; private set; }
        public long MaxWaterLevel { get; }
        public long MinWaterLevel { get; }
        public long MaxStagingLevel { get; }
        public decimal RecycleRate { get; }
        public decimal WaterFillPercent => (decimal)WaterLevel / MaxWaterLevel;

        public bool IsEmpty => WaterLevel == 0;

        public Type ParamType => typeof(TParam);
        public IParameters Param => WaterType;
        public Type ResultType => typeof(TResult);

        protected readonly IRandom800_90 Random;
        protected readonly IOracle Oracle;
        
        private readonly IOptions<PoolConfig> _poolConfig;
        private readonly IPoolRepository<TResult> _poolRepository;
        private readonly IPoolLogRepository _poolLogRepository;
        private readonly IPoolObjectFactory _poolObjectFactory;
        protected PoolBase(PoolConstructionParameters<TParam> param)
        {
            _poolConfig = param.PoolConfig;
            PoolName = param.PoolName;
            _poolRepository = param.PoolRepositoryFactory.GetRepository<TResult>();
            _poolLogRepository = param.PoolLogRepository;
            _poolObjectFactory = param.PoolObjectFactory;

            DeclaredType = param.PoolProperties.PoolType.Type;
            MaxWaterLevel = param.PoolProperties.MaxCapacity;
            MinWaterLevel = param.PoolProperties.MinCapacity;
            MaxStagingLevel = MinWaterLevel / 10;
            RecycleRate = param.PoolProperties.RecycleRatePerHundred / 100;
            WaterType = param.WaterType;

            Oracle = param.Oracle;

            Random = new Random800_90();

            WaterLevel = _poolRepository.GetPoolCount(PoolName, false);
        }

        public PoolResult<TResult> GetNext()
        {
            var startAction = DateTime.Now;

            if (WaterLevel < MinWaterLevel)
            {
                _poolLogRepository.WriteLog(LogTypes.PoolTooEmpty, PoolName, startAction, DateTime.Now, null);
                return new PoolResult<TResult> { PoolTooEmpty = true };
            }

            var result = _poolRepository.GetResultFromPool(PoolName);

            if (result != null)
            {
                var endAction = DateTime.Now;
                result.DateLastUsed = endAction;
                result.TimesUsed++;
                WaterLevel--;
                _poolLogRepository.WriteLog(LogTypes.GetValueFromPool, PoolName, startAction, endAction, null);

                RecycleValueWhenOptionsAllow(result);
                return new PoolResult<TResult>
                {
                    Result = result.Value
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
            _poolRepository.AddResultToPool(
                PoolName, 
                false, 
                _poolObjectFactory.WrapResult(value)
            );
            WaterLevel++;

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

        public bool AddWaterToStagingWater(PoolObject<TResult> value)
        {
            _poolRepository.AddResultToPool(PoolName, true, value);
            return true;
        }

        public bool CleanPool()
        {
            var startAction = DateTime.Now;
            _poolRepository.CleanPool(PoolName);
            WaterLevel = 0;
            _poolLogRepository.WriteLog(LogTypes.CleanPool, PoolName, startAction, DateTime.Now, null);
            return true;
        }

        private void RecycleValueWhenOptionsAllow(PoolObject<TResult> result)
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

                    // Check if staged water needs to be mixed in
                    if (_poolRepository.GetPoolCount(PoolName, true) > MaxStagingLevel)
                    {
                        var startAction = DateTime.Now;
                        _poolRepository.MixStagingPoolIntoPool(PoolName);
                        WaterLevel = _poolRepository.GetPoolCount(PoolName, false);
                        _poolLogRepository.WriteLog(LogTypes.MixStagingPool, PoolName, startAction, DateTime.Now, null);
                    }
                }
            }
        }

        public abstract Task RequestWater();
    }
}
