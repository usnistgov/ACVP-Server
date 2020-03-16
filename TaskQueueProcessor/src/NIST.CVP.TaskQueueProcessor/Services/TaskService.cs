using System.Threading.Tasks;
using NIST.CVP.TaskQueueProcessor.Providers;
using NIST.CVP.TaskQueueProcessor.TaskModels;
using Serilog;

namespace NIST.CVP.TaskQueueProcessor.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskProvider _taskProvider;
        private readonly IGenValService _genvalService;
        private readonly IPoolService _poolService;

        public TaskService(ITaskProvider taskProvider, IGenValService genValService, IPoolService poolService)
        {
            _taskProvider = taskProvider;
            _genvalService = genValService;
            _poolService = poolService;
        }

        public ExecutableTask GetTaskFromQueue()
        {
            // Basically everything in here is db reliant, so just let the provider handle it
            return _taskProvider.GetTaskFromQueue();
        }

        public async Task<long> RunTask(ExecutableTask task)
        {
            Log.Information($"Running job: {task.DbId}");

            // Stop executing method until you have a result from the task
            switch (task)
            {
                case GenerationTask generationTask:
                    await _genvalService.RunGenerator(generationTask);
                    break;
                
                case ValidationTask validationTask:
                    await _genvalService.RunValidator(validationTask);
                    break;
                
                case PoolTask poolTask:
                    await _poolService.SpawnPoolData();
                    break;
            }
            
            return task.DbId;
        }
    }
}