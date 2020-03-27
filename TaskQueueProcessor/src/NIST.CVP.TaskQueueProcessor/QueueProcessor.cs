using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.TaskQueueProcessor.Services;
using NIST.CVP.TaskQueueProcessor.TaskModels;
using Serilog;

namespace NIST.CVP.TaskQueueProcessor
{
    public class QueueProcessor : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ITaskService _taskService;
        private readonly ICleaningService _cleaningService;
        
        private readonly PoolConfig _poolConfig;
        private readonly TaskQueueProcessorConfig _taskConfig;

        private readonly SemaphoreSlim _semaphore;
        private readonly int _maxTasks;
        
        public QueueProcessor(IServiceProvider serviceProvider, ITaskService taskService, ICleaningService cleaningService, IOptions<PoolConfig> poolConfig, IOptions<TaskQueueProcessorConfig> taskConfig)
        {
            _serviceProvider = serviceProvider;
            _taskService = taskService;
            _cleaningService = cleaningService;
            _poolConfig = poolConfig.Value;
            _taskConfig = taskConfig.Value;

            _maxTasks = taskConfig.Value.MaxConcurrency;
            _semaphore = new SemaphoreSlim(_maxTasks, _maxTasks);
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_taskConfig.PollDelay * 1000 * 2, stoppingToken);
                await _semaphore.WaitAsync(stoppingToken);

                Log.Debug($"Grabbed a spot from the semaphore. Semaphore currently using {_maxTasks - _semaphore.CurrentCount} of {_maxTasks} threads.");

                // Grab the next task
                var task = _taskService.GetTaskFromQueue();
                //var task = GetMockTask();
                if (task != null)
                {
                    // Spawn a one-off task to run
                    Log.Information($"Grabbed dbId: {task.DbId}, vsId: {task.VsId} for gen/val processing");
                    QueueGenVal(stoppingToken, task).FireAndForget();
                    continue;
                }

                if (_poolConfig.AllowPoolSpawn && _semaphore.CurrentCount > 1)
                {
                    Log.Debug("No gen-val work to queue, starting pool population task.");
                    QueuePoolWork(stoppingToken).FireAndForget();
                    continue;
                }
                
                Log.Debug("No tasks available. Releasing the semaphore log and waiting.");
                _semaphore.Release();
                await Task.Delay(_taskConfig.PollDelay * 1000 * 2, stoppingToken);
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
                        
                    Log.Information($"Completed task {task.DbId}, VsId {task.VsId}");
                    _cleaningService.DeleteCompletedTask(task.DbId);
                }
                catch (Exception e)
                {
                    Log.Error(e, e.Message);
                }
                finally
                {
                    Log.Information("Generation task completed. Releasing Semaphore.");
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
                    Log.Debug("Pool task completed. Releasing Semaphore.");
                    _semaphore.Release();                        
                }
            }, cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop commands
            Log.Information("Stopping");
            _cleaningService.MarkTasksForRestart();
            Log.Information("Tasks saved");
            
            return base.StopAsync(cancellationToken);
        }
    }
}