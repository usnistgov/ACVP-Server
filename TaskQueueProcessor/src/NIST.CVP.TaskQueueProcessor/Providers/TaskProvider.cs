using System;
using Microsoft.Extensions.Logging;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Mighty;
using NIST.CVP.Libraries.Internal.TaskQueue;
using NIST.CVP.Libraries.Shared.ExtensionMethods;
using NIST.CVP.TaskQueueProcessor.Constants;
using NIST.CVP.TaskQueueProcessor.TaskModels;
using GenerationTask = NIST.CVP.TaskQueueProcessor.TaskModels.GenerationTask;
using ValidationTask = NIST.CVP.TaskQueueProcessor.TaskModels.ValidationTask;

namespace NIST.CVP.TaskQueueProcessor.Providers
{
    public class TaskProvider : ITaskProvider
    {
        private ILogger<TaskProvider> _logger;
        private readonly string _connectionString;
        private readonly IJsonProvider _jsonProvider;
        
        public TaskProvider(ILogger<TaskProvider> logger, IConnectionStringFactory connectionStringFactory, IJsonProvider jsonProvider)
        {
            _logger = logger;
            _connectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
            _jsonProvider = jsonProvider;
        }
        
        public ExecutableTask GetTaskFromQueue()
        {
            var acvp = new MightyOrm(_connectionString);
            var taskRow = acvp.SingleFromProcedure(StoredProcedures.GET_TASK_QUEUE);

            // If there is no row returned, then there is no data to process, do pool stuff
            if (taskRow == null)
                return null;
            
            // Parse out data into either generation or validation task
            var executableTask = BuildExecutableTask(taskRow);
            try
            {
                switch (executableTask)
                {
                    case GenerationTask generationTask:
                        generationTask.Capabilities = _jsonProvider.GetJson(generationTask.VsId, JsonFileTypes.CAPABILITIES);
                        return generationTask;
                
                    case ValidationTask validationTask:
                        validationTask.SubmittedResults = _jsonProvider.GetJson(validationTask.VsId, JsonFileTypes.SUBMITTED_RESULTS);
                        validationTask.InternalProjection = _jsonProvider.GetJson(validationTask.VsId, JsonFileTypes.INTERNAL_PROJECTION);
                        return validationTask;
                
                    default:
                        throw new Exception($"Invalid {nameof(executableTask)}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception encountered on building executable task.  Setting task to an error status.");
                SetTaskStatus(executableTask.DbId, TaskStatus.Error);
                return null;
            }
        }

        public void SetTaskStatus(long taskID, TaskStatus status)
        {
            var db = new MightyOrm(_connectionString);

            try
            {
                db.ExecuteProcedure("common.TaskQueueSetStatus", new
                {
                    TaskID = taskID,
                    Status = (int)status
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e);
            }
        }

        private ExecutableTask BuildExecutableTask(dynamic data)
        {
            string operation = data.TaskType;
            long dbId = data.TaskID;
            int vsId = (int)data.VsId;
            bool isSample = data.IsSample;
            bool showExpected = data.ShowExpected;

            return operation switch
            {
                TaskActions.GENERATION => new GenerationTask
                {
                    DbId = dbId, IsSample = isSample, VsId = vsId
                },

                TaskActions.VALIDATION => new ValidationTask
                {
                    DbId = dbId, Expected = showExpected, VsId = vsId
                },

                _ => throw new Exception($"Unknown task action: {operation}")
            };
        }
    }
}