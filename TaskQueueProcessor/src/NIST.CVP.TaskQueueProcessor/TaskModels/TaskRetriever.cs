using System;
using System.Data;
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
        
        public ExecutableTask GetTaskFromRow(IDataReader reader)
        {
            var dbId = reader.GetInt64(0);
            var operation = reader.GetString(1).ToLower();
            var vsIdJson = reader.GetString(2).ToLower();
            
            switch (operation)
            {
                case TaskActions.GENERATION:
                    var generationTask = JsonConvert.DeserializeObject<GenerationTask>(vsIdJson);
                    generationTask.GenValInvoker = _genValInvoker;
                    generationTask.DbId = dbId;
                    return generationTask;
                
                case TaskActions.VALIDATION:
                    var validationTask = JsonConvert.DeserializeObject<ValidationTask>(vsIdJson);
                    validationTask.GenValInvoker = _genValInvoker;
                    validationTask.DbId = dbId;
                    return validationTask;
                
                default:
                    throw new Exception($"Unknown task action: {operation}");
            }
        }
    }
}