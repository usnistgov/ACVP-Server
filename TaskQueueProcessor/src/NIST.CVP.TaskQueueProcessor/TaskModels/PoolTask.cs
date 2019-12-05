using System;
using NIST.CVP.TaskQueueProcessor.Providers;

namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public class PoolTask : ITask
    {
        public long DbId { get; set; } = -1;
        public int VsId { get; set; } = -1;

        private readonly IPoolProvider _poolProvider;
        
        /// <summary>
        /// Takes in a provider to the PoolApi and a boolean on whether or not to trigger an action
        /// </summary>
        /// <param name="poolProvider">Must be unique per PoolTask</param>
        public PoolTask(IPoolProvider poolProvider)
        {
            _poolProvider = poolProvider;
        }
        
        public void Run()
        {
            _poolProvider.SpawnPoolData();
            Console.WriteLine($"Pool Task");
        }
    }
}