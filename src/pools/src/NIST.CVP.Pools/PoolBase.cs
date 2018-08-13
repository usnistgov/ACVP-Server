using Newtonsoft.Json;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ResultTypes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace NIST.CVP.Pools
{
    public abstract class PoolBase<TParam, TResult>
        where TParam : IParameters
        where TResult : IResult
    {
        public TParam WaterType { get; }
        private readonly ConcurrentQueue<TResult> _water;
        public int WaterLevel => _water.Count;
        public bool IsEmpty => WaterLevel == 0;

        private readonly IList<JsonConverter> _jsonConverters;

        protected PoolBase(TParam waterType, string filename, IList<JsonConverter> jsonConverters)
        {
            WaterType = waterType;
            _jsonConverters = jsonConverters;
            _water = new ConcurrentQueue<TResult>();
            LoadPoolFromFile(filename);
        }

        public PoolResult<TResult> GetNext()
        {
            if (IsEmpty)
            {
                return new PoolResult<TResult> {PoolEmpty = true};
            }
            else
            {
                var success = _water.TryDequeue(out var result);
                if (success)
                {
                    return new PoolResult<TResult> { Result = result };
                }
                else
                {
                    throw new Exception("Unable to get next from queue");
                }
            }
        }

        public bool AddWater(TResult value)
        {
            _water.Enqueue(value);
            return true;
        }

        private void LoadPoolFromFile(string filename)
        {
            if (File.Exists(filename))
            {
                // Load file
                var poolContents = JsonConvert.DeserializeObject<TResult[]>(
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
                throw new Exception($"Unable to find pool at {filename}");
            }
        }

        public bool SavePoolToFile(string filename)
        {
            var poolContents = JsonConvert.SerializeObject(
                _water,
                new JsonSerializerSettings
                {
                    Converters = _jsonConverters
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
    }
}
