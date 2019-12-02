using System;
using NIST.CVP.TaskQueueProcessor.Providers;

namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public class PoolTask : ITask
    {
        public int DbId { get; set; } = -1;
        public int VsId { get; set; } = -1;
        public bool IsSample { get; set; }

        private readonly bool _allowPoolSpawn;
        private readonly IPoolProvider _poolProvider;
        
        /// <summary>
        /// Takes in a provider to the PoolApi and a boolean on whether or not to trigger an action
        /// </summary>
        /// <param name="poolProvider">Must be unique per PoolTask</param>
        /// <param name="allowPoolSpawn">Preferably set by config</param>
        public PoolTask(IPoolProvider poolProvider, bool allowPoolSpawn)
        {
            _poolProvider = poolProvider;
            _allowPoolSpawn = allowPoolSpawn;
        }
        
        public void Run()
        {
            if (_allowPoolSpawn)
            {
                _poolProvider.SpawnPoolData();
                Console.WriteLine($"Pool Task");
            }
        }
    }
}