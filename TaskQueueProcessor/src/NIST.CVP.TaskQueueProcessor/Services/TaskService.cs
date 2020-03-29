using System;
using System.Threading.Tasks;
using NIST.CVP.TaskQueueProcessor.Providers;
using NIST.CVP.TaskQueueProcessor.TaskModels;
using Serilog;

namespace NIST.CVP.TaskQueueProcessor.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskProvider _taskProvider;
        private readonly IGenValService _genValService;
        private readonly IPoolService _poolService;

        public TaskService(ITaskProvider taskProvider, IGenValService genValService, IPoolService poolService)
        {
            _taskProvider = taskProvider;
            _genValService = genValService;
            _poolService = poolService;
        }

        public ExecutableTask GetTaskFromQueue()
        {
            // Basically everything in here is db reliant, so just let the provider handle it
            return _taskProvider.GetTaskFromQueue();
        }

        public async Task RunTaskAsync(ExecutableTask task)
        {
            Log.Information($"Running job: {task.DbId}");
            var taskType = "";

            // Stop executing method until you have a result from the task
            try
            {
                switch (task)
                {
                    case GenerationTask generationTask:
                        taskType = "generation";
                        await _genValService.RunGeneratorAsync(generationTask);
                        return;

                    case ValidationTask validationTask:
                        taskType = "validation";
                        await _genValService.RunValidatorAsync(validationTask);
                        return;

                    case PoolTask poolTask:
                        taskType = "pool spawn";
                        await _poolService.SpawnPoolDataAsync();
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Exception on dbId: {task.DbId}, vsId: {task.VsId}, running: {taskType}");
                // No repeated throw, just exit out normally
            }
        }
    }
}