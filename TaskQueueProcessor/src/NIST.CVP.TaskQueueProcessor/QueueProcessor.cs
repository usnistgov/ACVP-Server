using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly List<long> _tasks = new List<long>();
        private int _poolTasks;
        private int _genValtasksRunning;
        private int _allTasks => _poolTasks + _genValtasksRunning;
        private int _maxTasks;
        
        private readonly ITaskService _taskService;
        private readonly ICleaningService _cleaningService;
        
        private readonly PoolConfig _poolConfig;
        private readonly TaskQueueProcessorConfig _taskConfig;
        
        public QueueProcessor(ITaskService taskService, ICleaningService cleaningService, IOptions<PoolConfig> poolConfig, IOptions<TaskQueueProcessorConfig> taskConfig)
        {
            _taskService = taskService;
            _cleaningService = cleaningService;
            _poolConfig = poolConfig.Value;
            _taskConfig = taskConfig.Value;

            _maxTasks = taskConfig.Value.MaxConcurrency;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(_taskConfig.PollDelay * 1000 * 2, stoppingToken);
            Log.Information("Starting Task Queue Processor");

            while (!stoppingToken.IsCancellationRequested)
            {
                // Check if there is room for a new job
                if (_genValtasksRunning == _taskConfig.MaxConcurrency)
                {
                    Log.Debug("Full on jobs. Re-poll.");
                }
                
                while (_allTasks < _maxTasks)
                {
                    // Grab the next task
                    var task = _taskService.GetTaskFromQueue();
                    
                    if (task != null)
                    {
                        // Spawn a one-off task to run
                        Log.Information($"Grabbed dbId: {task.DbId}, vsId: {task.VsId} for gen/val processing");
                        _genValtasksRunning++;
                        _tasks.Add(task.DbId);

                        try
                        {
                            QueueGenVal(task).FireAndForget();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                        
                    }
                    else if (_poolConfig.AllowPoolSpawn)    // task is always null at this point
                    {
                        // Pool spawning, if configured
                        Log.Information("Running pool spawn");
                        _poolTasks++;

                        var poolTask = Task.Factory.StartNew(() => _taskService.RunTask(new PoolTask()), stoppingToken);
                        poolTask.ContinueWith(OnPoolSpawnCompleted, stoppingToken);
                    }

                    // No more tasks available, wait for re-poll
                    if (task == null)
                    {
                        Log.Debug("No tasks available, wait for re-poll.");
                        break;
                    }
                    
                    Log.Debug("Polling delay within max concurrency loop");
                    await Task.Delay(_taskConfig.PollDelay * 1000, stoppingToken);
                }
                
                Log.Debug("Polling delay within main loop.");
                await Task.Delay(_taskConfig.PollDelay * 1000, stoppingToken);
            }
        }

        private async Task QueueGenVal(ExecutableTask task)
        {
            await _taskService.RunTaskAsync(task);
            
            Log.Information($"Completed task {task.DbId}, VsId {task.VsId}");
            _genValtasksRunning--;
            _tasks.Remove(task.DbId);
            _cleaningService.DeleteCompletedTask(task.DbId);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop commands
            Log.Information("Stopping");
            _cleaningService.MarkTasksForRestart();
            Log.Information("Tasks saved");
            
            return base.StopAsync(cancellationToken);
        }

        private void OnPoolSpawnCompleted(Task task)
        {
            _poolTasks--;
        }
    }
}