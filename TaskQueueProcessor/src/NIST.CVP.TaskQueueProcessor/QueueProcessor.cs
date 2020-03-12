using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.TaskQueueProcessor.Providers;
using NIST.CVP.TaskQueueProcessor.TaskModels;
using Serilog;

namespace NIST.CVP.TaskQueueProcessor
{
    public class QueueProcessor : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly Dictionary<long, Task> _tasks = new Dictionary<long, Task>();
        private readonly List<Task> _poolTasks = new List<Task>();
        
        private readonly ITaskRunner _taskRunner;
        private readonly IDbProvider _dbProvider;
        private readonly IPoolProvider _poolProvider;
        
        private readonly IOptions<PoolConfig> _poolConfig;
        private readonly IOptions<TaskQueueProcessorConfig> _taskConfig;

        public QueueProcessor(ITaskRunner taskRunner, IDbProvider dbProvider, IPoolProvider poolProvider, IOptions<PoolConfig> poolConfig, IOptions<TaskQueueProcessorConfig> taskConfig)
        {
            _taskRunner = taskRunner;
            _dbProvider = dbProvider;
            _poolProvider = poolProvider;
            _poolConfig = poolConfig;
            _taskConfig = taskConfig;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(
                (e) => PollForTask(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(_taskConfig.Value.PollDelay));
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Clean timer
            _timer?.Change(Timeout.Infinite, 0);

            // Started but incomplete jobs are marked in the table in the db as un-started to be picked up on run
            Log.Information("Stopping");
            _dbProvider.MarkTasksForRestart();
            Log.Information("Tasks saved");
            
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            // Dispose timer
            _timer?.Dispose();
        }

        public void CleanTasks()
        {
            try
            {
                var taskCount = _tasks.Count;
                for (var i = 0; i < taskCount; i++)
                {
                    // If no more tasks are left, just stop
                    if (!_tasks.Any())
                    {
                        return;
                    }
                    
                    // Find any finished task and remove it from the dict, and remove its ID from the DB (if successful)
                    var finished = Task.WhenAny(_tasks.Values.ToArray());
                    var completed = _tasks.First(t => t.Value == finished.Result);
                    var dbId = completed.Key;
                    _tasks.Remove(dbId);

                    if (!finished.IsCompletedSuccessfully)
                    {
                        Log.Error(finished.Exception, $"Task (dbId: {dbId}) did not complete successfully.");
                    }
                    else
                    {
                        Log.Debug($"Task (dbId: {dbId}) completed successfully.");
                        _dbProvider.DeleteCompletedTask(dbId);
                    }
                }
            }
            catch (ArgumentException)
            {
                // Nothing in the queue, just wait for a new Poll
                Log.Information("Queue is empty, waiting to poll");
            }
            catch (InvalidOperationException ex)
            {
                // This should never happen. Task.WhenAny would throw first under the same conditions that would cause this
                Log.Error(ex, "Something REALLY went wrong... Unable to remove task properly.");
            }
        }

        public void CleanPoolTasks()
        {
            try
            {
                var poolCount = _poolTasks.Count;
                for (var i = 0; i < poolCount; i++)
                {
                    var finished = Task.WhenAny(_poolTasks);
                    _poolTasks.Remove(finished.Result);
                    Log.Debug("Pool task completed");
                }
            }
            catch (ArgumentException)
            {
                // No pools being processed at the moment
            }
        }
        
        public void PollForTask()
        {
            CleanTasks();

            if (_poolConfig.Value.AllowPoolSpawn)
            {
                CleanPoolTasks();
            }
            
            var tasksAvailable = _taskConfig.Value.MaxConcurrency - _tasks.Count - _poolTasks.Count;
            if (tasksAvailable <= 0)
            {
                Log.Debug("Full on tasks.");
                return;
            }

            Log.Debug($"Polling db with {tasksAvailable} tasks available");
            while (tasksAvailable > 0)
            {
                Log.Debug($"Executable tasks: {_tasks.Count}. Pool tasks: {_poolTasks.Count}");

                var nextTask = _dbProvider.GetNextTask();

                if (nextTask != null)
                {
                    Log.Debug("Adding gen/val job.");
                    _tasks.Add(nextTask.DbId, _taskRunner.RunTask(nextTask));
                }
                else
                {
                    if (_poolConfig.Value.AllowPoolSpawn)
                    {
                        Log.Debug("Adding pool job.");
                        var poolTask = new PoolTask(_poolProvider);
                        _poolTasks.Add(_taskRunner.RunTask(poolTask));    
                    }
                    else
                    {
                        Log.Debug("No pool job to add.");
                        break;
                    }
                }

                tasksAvailable--;
            }
        }
    }
}