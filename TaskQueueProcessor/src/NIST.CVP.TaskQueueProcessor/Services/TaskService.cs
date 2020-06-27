using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.TaskQueue;
using NIST.CVP.Libraries.Internal.TaskQueue.Services;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.TaskQueueProcessor.TaskModels;
using Serilog;
using GenerationTask = NIST.CVP.TaskQueueProcessor.TaskModels.GenerationTask;
using TaskStatus = NIST.CVP.Libraries.Internal.TaskQueue.TaskStatus;
using ValidationTask = NIST.CVP.TaskQueueProcessor.TaskModels.ValidationTask;

namespace NIST.CVP.TaskQueueProcessor.Services
{
	public class TaskService : ITaskService
    {
        private readonly IGenValService _genValService;
        private readonly IPoolService _poolService;
        private readonly ITaskQueueService _taskQueueService;
        private readonly IVectorSetService _vectorSetService;
        private readonly ILogger<TaskService> _logger;

        public TaskService(IGenValService genValService, IPoolService poolService, ITaskQueueService taskQueueService, IVectorSetService vectorSetService, ILogger<TaskService> logger)
        {
            _genValService = genValService;
            _poolService = poolService;
            _taskQueueService = taskQueueService;
            _vectorSetService = vectorSetService;
            _logger = logger;
        }

        public ExecutableTask GetTaskFromQueue()
        {
            TaskQueueItem taskQueueItem = _taskQueueService.GetNext();
            try
            {
                return taskQueueItem?.Type switch
                {
                    TaskType.Generation => new GenerationTask()
                    {
                        DbId = taskQueueItem.ID,
                        VsId = taskQueueItem.VectorSetID,
                        IsSample = taskQueueItem.IsSample,
                        Capabilities = _vectorSetService.GetVectorFileJson(taskQueueItem.VectorSetID, VectorSetJsonFileTypes.Capabilities)
                    },
                    TaskType.Validation => new ValidationTask()
                    {
                        DbId = taskQueueItem.ID,
                        VsId = taskQueueItem.VectorSetID,
                        Expected = taskQueueItem.ShowExpected,
                        SubmittedResults = _vectorSetService.GetVectorFileJson(taskQueueItem.VectorSetID, VectorSetJsonFileTypes.SubmittedAnswers),
                        InternalProjection = _vectorSetService.GetVectorFileJson(taskQueueItem.VectorSetID, VectorSetJsonFileTypes.InternalProjection)
                    },
                    _ => null,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception encountered on building executable task.  Setting task to an error status.");
                _taskQueueService.UpdateStatus(taskQueueItem.ID, TaskStatus.Error);
                return null;
            }
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