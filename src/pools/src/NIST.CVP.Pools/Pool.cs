using Newtonsoft.Json;
using NIST.CVP.Generation.Core.JsonConverters;
using System.Collections.Generic;
using System.IO;

namespace NIST.CVP.Pools
{
    public class Pool<TParam, TResult>
    {
        public TParam WaterType { get; }

        private readonly Queue<TResult> _water;
        public int WaterLevel => _water.Count;
        public double WaterLevelPercent => WaterLevel / (double) MaxWaterLevel;
        public int MaxWaterLevel { get; set; }
        public bool IsEmpty => WaterLevel == 0;
        public int MonitorFrequency { get; set; }

        // TODO load from config.json file?
        //private const int MONITOR_FREQUENCY = 300; // seconds
        //private const int MAX_WATER_LEVEL = 1000;

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

                foreach (var result in poolContents)
                {
                    _water.Enqueue(result);
                }
            }
            else
            {
                File.Create(filename);
            }
        }

        public void SavePoolToFile(string filename)
        {
            // Store some meta data about the pool
            // Content Type
            // Max Capacity

            // Store actual content of pool
        }
    }
}
