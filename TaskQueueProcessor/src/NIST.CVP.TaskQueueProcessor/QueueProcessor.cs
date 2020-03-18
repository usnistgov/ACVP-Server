using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.TaskQueueProcessor.Services;
using NIST.CVP.TaskQueueProcessor.TaskModels;
using Serilog;

namespace NIST.CVP.TaskQueueProcessor
{
    public class QueueProcessor : BackgroundService
    {
        private readonly List<long> _tasks = new List<long>();
        private int _poolTasks = 0;
        
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
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(_taskConfig.PollDelay * 1000 * 2, stoppingToken);
            Log.Information("Starting Task Queue Processor");

            while (!stoppingToken.IsCancellationRequested)
            {
                // Check if there is room for a new job
                var tasksRunning = _tasks.Count + _poolTasks;    // Don't use actual lists for count so tasksRunning only increases within the loop
                if (tasksRunning == _taskConfig.MaxConcurrency)
                {
                    Log.Debug("Full on jobs. Re-poll.");
                }
                
                while (tasksRunning < _taskConfig.MaxConcurrency)
                {
                    // Grab the next task
                    var task = _taskService.GetTaskFromQueue();
                    
                    if (task != null)
                    {
                        // Spawn a one-off task to run
                        Log.Information($"Grabbed dbId: {task.DbId}, vsId: {task.VsId} for gen/val processing");
                        tasksRunning++;
                        _tasks.Add(task.DbId);
                        
                        var genValTask = Task.Factory.StartNew(() => _taskService.RunTask(task), stoppingToken);
                        genValTask.ContinueWith(OnGenValCompleted, task, stoppingToken);
                    }
                    else if (_poolConfig.AllowPoolSpawn)    // task is always null at this point
                    {
                        // Pool spawning, if configured
                        Log.Information("Running pool spawn");
                        tasksRunning++;
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
                }
                
                await Task.Delay(_taskConfig.PollDelay * 1000, stoppingToken);
            }
            
            // Stop commands
            MarkTasksForRestart();
        }

        private void OnGenValCompleted(Task thread, object executableTask)
        {
            if (executableTask is ExecutableTask completedTask)
            {
                _tasks.Remove(completedTask.DbId);
            }
        }

        private void OnPoolSpawnCompleted(Task task)
        {
            _poolTasks--;
        }

        private void MarkTasksForRestart()
        {
            Log.Information("Stopping");
            _cleaningService.MarkTasksForRestart();
            Log.Information("Tasks saved");
        }
    }
}