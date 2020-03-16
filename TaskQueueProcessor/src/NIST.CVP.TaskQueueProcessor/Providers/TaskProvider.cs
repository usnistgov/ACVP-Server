using System;
using System.Linq;
using ACVPCore.ExtensionMethods;
using CVP.DatabaseInterface;
using Mighty;
using NIST.CVP.TaskQueueProcessor.Constants;
using NIST.CVP.TaskQueueProcessor.TaskModels;

namespace NIST.CVP.TaskQueueProcessor.Providers
{
    public class TaskProvider : ITaskProvider
    {
        private readonly string _connectionString;
        private readonly IJsonProvider _jsonProvider;
        
        public TaskProvider(IConnectionStringFactory connectionStringFactory, IJsonProvider jsonProvider)
        {
            _connectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
            _jsonProvider = jsonProvider;
        }
        
        public ExecutableTask GetTaskFromQueue()
        {
            ExecutableTask task;
            var acvp = new MightyOrm(_connectionString);
            var taskRow = acvp.QueryWithExpando(StoredProcedures.GET_TASK_QUEUE);

            // If there is no row returned, then there is no data to process, do pool stuff
            if (!taskRow.Data.Any())
                return null;
            
            // Parse out data into either generation or validation task
            var executableTask = BuildExecutableTask(taskRow.ResultsExpando);
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
                    throw new Exception();
            }
        }

        private ExecutableTask BuildExecutableTask(dynamic data)
        {
            string operation = data.Operation;
            long dbId = data.Id;
            int vsId = (int)data.VsID;
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