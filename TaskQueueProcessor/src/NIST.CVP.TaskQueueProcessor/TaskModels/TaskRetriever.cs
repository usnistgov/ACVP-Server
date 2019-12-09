using System;
using System.Data.SqlClient;
using Newtonsoft.Json;
using NIST.CVP.TaskQueueProcessor.Constants;

namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public class TaskRetriever
    {
        public ExecutableTask GetTaskFromRow(SqlDataReader reader)
        {
            var dbId = reader.GetInt32(0);    // TODO verify type here? Is this a BIGINT? Long?
            var operation = reader.GetString(1).ToLower();
            var vsIdSampleJson = reader.GetString(2).ToLower();

            // TODO these don't have to be JSON in the database...
            var executableTask = JsonConvert.DeserializeObject<ExecutableTask>(vsIdSampleJson);
            executableTask.DbId = dbId;
            
            switch (operation)
            {
                case TaskActions.GENERATION:
                    return executableTask as GenerationTask;
                case TaskActions.VALIDATION:
                    return executableTask as ValidationTask;
                default:
                    throw new Exception($"Unknown task action: {operation}");
            }
        }
    }
}