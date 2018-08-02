using System.Collections.Generic;
using System.IO;

namespace NIST.CVP.Common.Oracle
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
            if (Directory.Exists(filename))
            {
                // Load file
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
