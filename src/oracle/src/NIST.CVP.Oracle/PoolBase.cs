using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Common.Oracle
{
    public abstract class PoolBase<T>
    {
        protected Queue<T> Water { get; }
        public int WaterLevel => Water.Count;
        public int MaxWaterLevel { get; private set; }
        public bool IsEmpty => WaterLevel == 0;

        private const double LOWER_RATIO = .05;
        private const double UPPER_RATIO = .95;
        private const double GROWTH_RATIO = .10;
        private const double SHRINK_RATIO = .05;

        protected PoolBase()
        {
            Water = new Queue<T>();
        }

        protected PoolBase(Queue<T> water)
        {
            Water = water;
        }

        protected PoolBase(string filename)
        {
            // Load pool from file
        }

        public T GetNext()
        {
            if (IsEmpty)
            {
                // Generate it, skip the pool
                throw new NotImplementedException();
            }
            else
            {
                return Water.Dequeue();
            }
        }

        // Goal is to have pools sit at around 80% capacity while being constantly depleted.
        public void AdaptPool()
        {
            // If there isn't enough water in the pool,
            //  then the pool is being depleted too quickly,
            //  so grow the pool in response and let it fill up.
            // Quickly queue up new jobs to fill in the missing water.
            if (WaterLevel < LOWER_RATIO * MaxWaterLevel)
            {
                MaxWaterLevel += (int) (MaxWaterLevel * GROWTH_RATIO);

                // TODO add more water to the pool

                return;
            }

            // If there is too much water in the pool,
            //  then shrink the pool a bit so no new jobs
            //  get queue up and time can be spent elsewhere.
            // Extra values can sit in the pool and deplete naturally.
            if (WaterLevel > UPPER_RATIO * MaxWaterLevel)
            {
                MaxWaterLevel -= (int) (MaxWaterLevel * SHRINK_RATIO);
                return;
            }
        }
    }
}
