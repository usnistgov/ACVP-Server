using System;
using System.Data;
using NIST.CVP.Generation.Core;
using NIST.CVP.TaskQueueProcessor.Constants;
using NIST.CVP.TaskQueueProcessor.Providers;

namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public class TaskRetriever : ITaskRetriever
    {
        private readonly IGenValInvoker _genValInvoker;
        
        public TaskRetriever(IGenValInvoker genValInvoker)
        {
            _genValInvoker = genValInvoker;
        }
        
        public ExecutableTask GetTaskFromRow(IDataReader reader, IDbProvider dbProvider)
        {
            var dbId = reader.GetInt64(0);
            var operation = reader.GetString(1).ToLower();
            var vsId = reader.GetInt64(2);
            var isSample = reader.GetBoolean(3);
            var showExpected = reader.GetBoolean(4);

            return operation switch
            {
                TaskActions.GENERATION => new GenerationTask(_genValInvoker, dbProvider)
                {
                    DbId = dbId, IsSample = isSample, VsId = (int)vsId
                },
                
                TaskActions.VALIDATION => new ValidationTask(_genValInvoker, dbProvider)
                {
                    DbId = dbId, Expected = showExpected, VsId = (int)vsId
                },
                
                _ => throw new Exception($"Unknown task action: {operation}")
            };
        }
    }
}