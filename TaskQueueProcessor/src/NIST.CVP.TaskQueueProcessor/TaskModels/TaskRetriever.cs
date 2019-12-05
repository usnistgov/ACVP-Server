using System;
using System.Data.SqlClient;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.TaskQueueProcessor.Constants;

namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public class TaskRetriever : ITaskRetriever
    {
        private readonly IGenValInvoker _genValInvoker;
        
        public TaskRetriever(IGenValInvoker genValInvoker)
        {
            _genValInvoker = genValInvoker;
        }
        
        public ExecutableTask GetTaskFromRow(SqlDataReader reader)
        {
            var dbId = reader.GetInt64(0);    // TODO verify type here? Is this a BIGINT? Long?
            var operation = reader.GetString(1).ToLower();
            var vsIdJson = reader.GetString(2).ToLower();

            // TODO these don't have to be JSON in the database...
            //var executableTask = JsonConvert.DeserializeObject<ExecutableTask>(vsIdJson);
            //executableTask.DbId = dbId;

            switch (operation)
            {
                case TaskActions.GENERATION:
                    var generationTask = JsonConvert.DeserializeObject<GenerationTask>(vsIdJson);
                    generationTask.DbId = dbId;
                    return generationTask;
                
                case TaskActions.VALIDATION:
                    var validationTask = JsonConvert.DeserializeObject<ValidationTask>(vsIdJson);
                    validationTask.DbId = dbId;
                    return validationTask;
                
                default:
                    throw new Exception($"Unknown task action: {operation}");
            }
        }
    }
}