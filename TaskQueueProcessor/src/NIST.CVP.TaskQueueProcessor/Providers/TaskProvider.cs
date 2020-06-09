using System;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.TaskQueue;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
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
        private readonly IVectorSetService _vectorSetService;
        
        public TaskProvider(ILogger<TaskProvider> logger, IConnectionStringFactory connectionStringFactory, IVectorSetService vectorSetService)
        {
            _logger = logger;
            _connectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
            _vectorSetService = vectorSetService;
        }
        
        public ExecutableTask GetTaskFromQueue()
        {
            var acvp = new MightyOrm(_connectionString);
            var taskRow = acvp.SingleFromProcedure(StoredProcedures.GET_TASK_QUEUE);

            // If there is no row returned, then there is no data to process, do pool stuff
            if (taskRow == null)
                return null;
            
            string operation = taskRow.TaskType;
            long dbId = taskRow.TaskID;
            long vsId = taskRow.VsId;
            bool isSample = taskRow.IsSample;
            bool showExpected = taskRow.ShowExpected;
            
            // Parse out data into either generation or validation task
            try
            {
                switch (operation)
                {
                    case TaskActions.GENERATION:

                        return new GenerationTask()
                        {
                            DbId = dbId,
                            VsId = vsId,
                            IsSample = isSample,
                            Capabilities = GetJson(vsId, VectorSetJsonFileTypes.Capabilities)
                        };
                    case TaskActions.VALIDATION:
                        
                        return new ValidationTask()
                        {
                            DbId = dbId, 
                            VsId = vsId,
                            Expected = showExpected, 
                            SubmittedResults = GetJson(vsId, VectorSetJsonFileTypes.SubmittedAnswers),
                            InternalProjection = GetJson(vsId, VectorSetJsonFileTypes.InternalProjection)
                        };
                    default:
                        throw new Exception($"Invalid {nameof(operation)}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception encountered on building executable task.  Setting task to an error status.");
                SetTaskStatus(dbId, TaskStatus.Error);
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
        
        private string GetJson(long vsId, VectorSetJsonFileTypes fileType)
        {
            var json = _vectorSetService.GetVectorFileJson(vsId, fileType);

            if (string.IsNullOrEmpty(json))
            {
                throw new Exception($"Json for vsId {vsId}, fileType {fileType} was null.");
            }

            return json;
        }
    }
}