using System;
using System.Threading.Tasks;
using NIST.CVP.TaskQueueProcessor.Providers;

namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public class PoolTask : ITask
    {
        public long DbId { get; set; } = -1;
        public int VsId { get; set; } = -1;

        private readonly IPoolProvider _poolProvider;

        public PoolTask(IPoolProvider poolProvider)
        {
            _poolProvider = poolProvider;
        }
        
        public async Task<object> Run()
        {
            Console.WriteLine($"Pool Task");
            return await _poolProvider.SpawnPoolData();
        }
    }
}