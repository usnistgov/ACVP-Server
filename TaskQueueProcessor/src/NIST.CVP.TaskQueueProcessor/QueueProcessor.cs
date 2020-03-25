using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NIST.CVP.Common;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.TaskQueueProcessor.Services;
using NIST.CVP.TaskQueueProcessor.TaskModels;
using Serilog;

namespace NIST.CVP.TaskQueueProcessor
{
    public class QueueProcessor : BackgroundService
    {
        private readonly ITaskService _taskService;
        private readonly ICleaningService _cleaningService;
        
        private readonly PoolConfig _poolConfig;
        private readonly TaskQueueProcessorConfig _taskConfig;

        private readonly SemaphoreSlim _semaphore;
        private readonly int _maxTasks;
        private readonly LimitedConcurrencyLevelTaskScheduler _scheduler;

        public QueueProcessor(ITaskService taskService, ICleaningService cleaningService, IOptions<PoolConfig> poolConfig, IOptions<TaskQueueProcessorConfig> taskConfig, LimitedConcurrencyLevelTaskScheduler scheduler)
        {
            _taskService = taskService;
            _cleaningService = cleaningService;
            _poolConfig = poolConfig.Value;
            _taskConfig = taskConfig.Value;

            _maxTasks = taskConfig.Value.MaxConcurrency + 1;
            _maxTasks = 1;
            _semaphore = new SemaphoreSlim(_maxTasks, _maxTasks);
            _scheduler = scheduler;
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
                
                    await QueueGenVal(task, stoppingToken);
                    continue;
                }
                
                Log.Debug("No tasks available. Releasing the semaphore log and waiting.");
                _semaphore.Release();
                await Task.Delay(_taskConfig.PollDelay * 1000 * 2, stoppingToken);
                // TODO pool spawn.
            }
        }
        
        private Task QueueGenVal(ExecutableTask task, CancellationToken stoppingToken)
        {
            try
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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
            _semaphore.Release();
        }

        private ExecutableTask GetMockTask()
        {
            #region
            var capabilities = @"{
      ""vsId"" : 10134,
      ""isSample"" : true,
      ""algorithm"" : ""KAS-ECC"",
      ""revision"" : ""Sp800-56Ar3"",
      ""iutId"" : ""123456ABCD"",
      ""scheme"" : {
        ""onePassDh"" : {
          ""kasRole"" : [ ""initiator"", ""responder"" ],
          ""kdfMethods"" : {
            ""oneStepKdf"" : {
              ""auxFunctions"" : [ {
                ""auxFunctionName"" : ""KMAC-128"",
                ""macSaltMethods"" : [ ""default"" ]
              } ],
              ""fixedInfoPattern"" : ""algorithmId||l||uPartyInfo||vPartyInfo"",
              ""encoding"" : [ ""concatenation"" ]
            },
            ""twoStepKdf"" : {
              ""capabilities"" : [ {
                ""macSaltMethods"" : [ ""random"" ],
                ""fixedInfoPattern"" : ""l||label||uPartyInfo||vPartyInfo||context"",
                ""encoding"" : [ ""concatenation"" ],
                ""kdfMode"" : ""feedback"",
                ""macMode"" : [ ""HMAC-SHA3-224"" ],
                ""supportedLengths"" : [ 512 ],
                ""fixedDataOrder"" : [ ""after fixed data"" ],
                ""counterLength"" : [ 32 ],
                ""requiresEmptyIv"" : false,
                ""supportsEmptyIv"" : false
              } ]
            }
          },
          ""keyConfirmationMethod"" : {
            ""macMethods"" : {
              ""KMAC-128"" : {
                ""keyLen"" : 128,
                ""macLen"" : 128
              }
            },
            ""keyConfirmationDirections"" : [ ""unilateral"" ],
            ""keyConfirmationRoles"" : [ ""provider"", ""recipient"" ]
          },
          ""l"" : 512
        }
      },
      ""domainParameterGenerationMethods"" : [ ""P-192"" ]
    }";
            #endregion

            return new GenerationTask()
            {
                Capabilities = capabilities,
                DbId = 1,
                VsId = 1
            };
        }
    }
}