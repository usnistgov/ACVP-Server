using Newtonsoft.Json;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.Pools.Models;

namespace NIST.CVP.Pools
{
    public abstract class PoolBase<TParam, TResult> : IPool<TParam, TResult>
        where TParam : IParameters
        where TResult : IResult
    {
        public PoolTypes DeclaredType { get; }
        public TParam WaterType { get; }
        
        public int WaterLevel => _water.Count;
        public bool IsEmpty => WaterLevel == 0;

        public Type ParamType => typeof(TParam);
        public IParameters Param => WaterType;
        public Type ResultType => typeof(TResult);

        private readonly ConcurrentQueue<ResultWrapper<TResult>> _water;
        private readonly IList<JsonConverter> _jsonConverters;
        private readonly IOptions<PoolConfig> _poolConfig;

        

        protected PoolBase(IOptions<PoolConfig> poolConfig, PoolTypes declaredType, TParam waterType, string filename, IList<JsonConverter> jsonConverters)
        {
            _poolConfig = poolConfig;
            DeclaredType = declaredType;
            WaterType = waterType;
            _jsonConverters = jsonConverters;
            _water = new ConcurrentQueue<ResultWrapper<TResult>>();
            LoadPoolFromFile(filename);
        }

        public PoolResult<TResult> GetNext()
        {
            if (IsEmpty)
            {
                return new PoolResult<TResult> { PoolEmpty = true };
            }
            else
            {
                var success = _water.TryDequeue(out var wrappedResult);
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

        private void LoadPoolFromFile(string filename)
        {
            if (File.Exists(filename))
            {
                // Load file
                var poolContents = JsonConvert.DeserializeObject<ResultWrapper<TResult>[]>(
                    File.ReadAllText(filename),
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
                File.WriteAllText(filename, "[]");
                LogManager.GetCurrentClassLogger().Debug($"{filename} created");
            }
        }

        public bool SavePoolToFile(string filename)
        {
            var poolContents = JsonConvert.SerializeObject(
                _water,
                new JsonSerializerSettings
                {
                    Converters = _jsonConverters,
                    Formatting = Formatting.Indented
                }
            );

            try
            {
                File.WriteAllText(filename, poolContents);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CleanPool()
        {
            _water.Clear();
            return true;
        }

        private void RecycleValueWhenOptionsAllow(ResultWrapper<TResult> result)
        {
            if (_poolConfig.Value.ShouldRecyclePoolWater)
            {
                var newResultToQueue = new ResultWrapper<TResult>()
                {
                    Result = result.Result,
                    TimesValueUsed = result.TimesValueUsed + 1,
                    ValueCreated = result.ValueCreated,
                    ValueUsed = DateTime.UtcNow
                };

                AddWater(newResultToQueue);
            }
        }

        private bool AddWater(ResultWrapper<TResult> result)
        {
            _water.Enqueue(result);
            return true;
        }
    }
}
