using System;
using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Common.Oracle
{
    public class Pool<T>
    {
        public ContentType WaterType { get; }

        private readonly Queue<T> _water;
        public int WaterLevel => _water.Count;
        public double WaterLevelPercent => WaterLevel / (double) MaxWaterLevel;
        public int MaxWaterLevel { get; set; } = MAX_WATER_LEVEL;
        public bool IsEmpty => WaterLevel == 0;

        public int MonitorFrequency => MONITOR_FREQUENCY;

        public int ExpectedFillTime { get; private set; }

        // TODO load from config.json file?
        private const int MONITOR_FREQUENCY = 300; // seconds
        private const int MAX_WATER_LEVEL = 1000;

        public Pool(int expectedFillTime, int maxCapacity)
        {
            _water = new Queue<T>();
            ExpectedFillTime = expectedFillTime;
            MaxWaterLevel = maxCapacity;
        }

        public Pool(Queue<T> water, int fillTime)
        {
            _water = water;
            ExpectedFillTime = fillTime;
        }

        public Pool(string filename)
        {
            _water = new Queue<T>();
            LoadPoolFromFile(filename);
        }

        public IEnumerable<T> GetNext(int amountToGet)
        {
            if (IsEmpty || amountToGet > WaterLevel)
            {
                // Generate it, skip the pool, but show others you need more data
                MaxWaterLevel *= 2;
                _water.Clear();
                return Enumerable.Range(0, amountToGet).Select(s => default(T));
            }
            else
            {
                return Enumerable.Range(0, amountToGet).Select(s => _water.Dequeue());
            }
        }

        public void AddWater(T value)
        {
            _water.Enqueue(value);
        }

        private void LoadPoolFromFile(string filename)
        {

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
