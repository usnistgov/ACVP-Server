using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NIST.CVP.TaskQueueProcessor.Providers;
using NIST.CVP.TaskQueueProcessor.TaskModels;

namespace NIST.CVP.TaskQueueProcessor
{
    public class QueueProcessor : IHostedService, IDisposable
    {
        private const int POLL_DELAY = 5;
        private const int CONCURRENT_TASK_LIMIT = 4;
        private const string CONNECTION_STRING = "Server=localhost;Database=Acvp;User=SA;Password=Password123;";
        private const bool ALLOW_POOL_SPAWN = true;
        private const string POOL_URL = "localhost";
        private const int POOL_PORT = 5002;

        private Timer _timer;
        private readonly Dictionary<int, Task> _tasks = new Dictionary<int, Task>();    // TODO how big is SQL BIGINT ? 
        private readonly List<Task> _poolTasks = new List<Task>();
        private readonly ITaskRunner _taskRunner;
        private readonly IDbProvider _dbProvider;
        private readonly IPoolProvider _poolProvider;

        public QueueProcessor()
        {
            _taskRunner = new TaskRunner();
            _dbProvider = new DbProvider(CONNECTION_STRING);
            _poolProvider = new PoolProvider(POOL_URL, POOL_PORT);
        }

        public QueueProcessor(ITaskRunner taskRunner, IDbProvider dbProvider, IPoolProvider poolProvider)
        {
            _taskRunner = taskRunner;
            _dbProvider = dbProvider;
            _poolProvider = poolProvider;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(
                (e) => PollForTask(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(POLL_DELAY));
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Clean timer
            _timer?.Change(Timeout.Infinite, 0);

            // Started but incomplete jobs are marked in the table in the db as un-started to be picked up on run
            Console.WriteLine("Stopping");
            _dbProvider.MarkTasksForRestart();
            Console.WriteLine("Tasks saved");
            
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            // Dispose timer
            _timer?.Dispose();
        }

        private async void CleanTasks()
        {
            try
            {
                var taskCount = _tasks.Count;
                for (var i = 0; i < taskCount; i++)
                {
                    // Find any finished task and remove it from the dict, and remove its ID from the DB (if successful)
                    var finished = await Task.WhenAny(_tasks.Values);
                    var dbId = _tasks.First(t => t.Value == finished).Key;
                    _tasks.Remove(dbId);

                    if (!finished.IsCompletedSuccessfully)
                    {
                        Console.WriteLine($"Task {dbId} did not complete successfully. {finished.Exception}");
                    }
                    else
                    {
                        _dbProvider.DeleteCompletedTask(dbId);
                    }
                }
            }
            catch (ArgumentException)
            {
                // Nothing in the queue, just wait for a new Poll
                Console.WriteLine("Queue is empty, waiting to poll");
            }
            catch (InvalidOperationException ex)
            {
                // This should never happen. Task.WhenAny would throw first under the same conditions that would cause this
                Console.WriteLine("Something REALLY went wrong...");
                Console.WriteLine(ex);
            }
        }

        private async void CleanPoolTasks()
        {
            try
            {
                var poolCount = _poolTasks.Count;
                for (var i = 0; i < poolCount; i++)
                {
                    var finished = await Task.WhenAny(_poolTasks);
                    _poolTasks.Remove(finished);
                }
            }
            catch (ArgumentException)
            {
                // No pools being processed at the moment
            }
        }
        
        private void PollForTask()
        {
            CleanTasks();

            if (ALLOW_POOL_SPAWN)
            {
                CleanPoolTasks();
            }
            
            var tasksAvailable = CONCURRENT_TASK_LIMIT - _tasks.Count - _poolTasks.Count;
            if (tasksAvailable <= 0)
            {
                return;
            }

            Console.WriteLine($"Polling db with {tasksAvailable} tasks available");
            while (tasksAvailable > 0)
            {
                var nextTask = _dbProvider.GetNextTask();

                if (nextTask != null)
                {
                    _tasks.Add(nextTask.DbId, _taskRunner.RunTask(nextTask));
                }
                else
                {
                    if (ALLOW_POOL_SPAWN)
                    {
                        var poolTask = new PoolTask(_poolProvider);
                        _poolTasks.Add(_taskRunner.RunTask(poolTask));    
                    }
                }
                
                tasksAvailable--;
            }
        }
    }
}