using Newtonsoft.Json;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core.JsonConverters;
using System.Collections.Generic;
using System.IO;

namespace NIST.CVP.Pools
{
    public class Pool<TParam, TResult>
        where TParam : Parameters
    {
        public TParam WaterType { get; }
        private readonly Queue<TResult> _water;
        public int WaterLevel => _water.Count;
        public bool IsEmpty => WaterLevel == 0;

        private readonly IList<JsonConverter> _jsonConverters = new List<JsonConverter>
        {
            new BitstringConverter(),
            new DomainConverter(),
            new BigIntegerConverter()
        };

        public Pool(TParam waterType, string filename)
        {
            WaterType = waterType;
            _water = new Queue<TResult>();
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
                return new PoolResult<TResult> {Result = _water.Dequeue()};
            }
        }

        public void AddWater(TResult value)
        {
            _water.Enqueue(value);
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
                File.Create(filename);
            }
        }

        public void SavePoolToFile(string filename)
        {
            var poolContents = JsonConvert.SerializeObject(
                _water,
                new JsonSerializerSettings
                {
                    Converters = _jsonConverters
                }
            );

            File.WriteAllText(filename, poolContents);
        }
    }
}
