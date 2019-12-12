using System;
using System.Data;
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
        
        public ExecutableTask GetTaskFromRow(IDataReader reader)
        {
            var dbId = reader.GetInt64(0);
            var operation = reader.GetString(1).ToLower();
            var vsId = reader.GetInt32(2);
            var isSample = reader.GetInt32(3) == 1;            // Boolean
            var showExpected = reader.GetInt32(4) == 1;        // Boolean

            return operation switch
            {
                TaskActions.GENERATION => new GenerationTask(_genValInvoker)
                {
                    DbId = dbId, IsSample = isSample, VsId = vsId
                },
                
                TaskActions.VALIDATION => new ValidationTask(_genValInvoker)
                {
                    DbId = dbId, Expected = showExpected, VsId = vsId
                },
                
                _ => throw new Exception($"Unknown task action: {operation}")
            };
        }
    }
}