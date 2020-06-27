using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Libraries.Internal.TaskQueue.Services;
using NIST.CVP.TaskQueueProcessor.Services;
using NIST.CVP.TaskQueueProcessor.TaskModels;
using Serilog;

namespace NIST.CVP.TaskQueueProcessor
{
	public class QueueProcessor : BackgroundService
    {
        private readonly ITaskService _taskService;
        private readonly ITaskQueueService _taskQueueService;
        
        private readonly PoolConfig _poolConfig;
        private readonly TaskQueueProcessorConfig _taskConfig;

        private readonly SemaphoreSlim _semaphore;
        private readonly int _maxTasks;
        
        public QueueProcessor(ITaskService taskService, IOptions<PoolConfig> poolConfig, IOptions<TaskQueueProcessorConfig> taskConfig)
        {
            _taskService = taskService;
            _poolConfig = poolConfig.Value;
            _taskConfig = taskConfig.Value;

            _maxTasks = taskConfig.Value.MaxConcurrency;
            _semaphore = new SemaphoreSlim(_maxTasks, _maxTasks);
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Log.Information("Starting Task Queue Processor");

            while (!stoppingToken.IsCancellationRequested)
            {
                await _semaphore.WaitAsync(stoppingToken);

                Log.Debug($"Grabbed a spot from the semaphore. Semaphore currently using {_maxTasks - _semaphore.CurrentCount} of {_maxTasks} threads.");

                // Grab the next task
                var task = _taskService.GetTaskFromQueue();
                var taskQueued = false;
                //var task = GetMockTask();
                if (task != null)
                {
                    taskQueued = true;
                    
                    // Spawn a one-off task to run
                    Log.Information($"Grabbed dbId: {task.DbId}, vsId: {task.VsId} for gen/val processing. Semaphore currently using {_maxTasks - _semaphore.CurrentCount} of {_maxTasks} threads.");
                    QueueGenVal(stoppingToken, task).FireAndForget();
                }

                if (_poolConfig.AllowPoolSpawn && _semaphore.CurrentCount > 1)
                {
                    taskQueued = true;
                    
                    Log.Information($"No gen-val work to queue, starting pool population task. Semaphore currently using {_maxTasks - _semaphore.CurrentCount} of {_maxTasks} threads.");
                    QueuePoolWork(stoppingToken).FireAndForget();
                }

                if (!taskQueued)
                {
                    Log.Debug("No tasks available. Releasing the semaphore log and waiting.");
                    _semaphore.Release();
                    await Task.Delay(_taskConfig.PollDelay * 1000, stoppingToken);    
                }
            }
        }
        
        private Task QueueGenVal(CancellationToken stoppingToken, ExecutableTask task)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var genValTask = _taskService.RunTaskAsync(task);

                    await genValTask;
                    _taskQueueService.Delete(task.DbId);
                }
                catch (Exception e)
                {
                    Log.Error(e, e.Message);
                }
                finally
                {
                    Log.Information($"{task.GetType()} task completed DbId {task.DbId}, VsId {task.VsId}.");
                    _semaphore.Release();                        
                }
            }, stoppingToken);
        }

        private Task QueuePoolWork(CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var poolTask = _taskService.RunTaskAsync(new PoolTask());

                    await poolTask;
                }
                catch (Exception e)
                {
                    Log.Error(e, e.Message);
                }
                finally
                {
                    Log.Information($"Pool task completed.");
                    _semaphore.Release();                        
                }
            }, cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop commands
            Log.Information("Stopping");
            _taskQueueService.RestartAll();
            Log.Information("Tasks saved");
            
            return base.StopAsync(cancellationToken);
        }
    }
}