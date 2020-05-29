using System;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Mighty;
using NIST.CVP.TaskQueueProcessor.Constants;
using Serilog;

namespace NIST.CVP.TaskQueueProcessor.Services
{
    public class CleaningService : ICleaningService
    {
        private readonly string _connectionString;
        
        public CleaningService(IConnectionStringFactory connectionStringFactory)
        {
            _connectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
        }
        
        public void DeleteCompletedTask(long taskId)
        {
            var acvp = new MightyOrm(_connectionString);

            try
            {
                acvp.ExecuteProcedure(StoredProcedures.DELETE_TASK_FROM_TASK_QUEUE, new
                {
                    TaskID = taskId
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Unable to remove task dbId: {taskId} from Task Queue");
                throw;
            }
        }

        public void MarkTasksForRestart()
        {
            var acvp = new MightyOrm(_connectionString);

            try
            {
                acvp.ExecuteProcedure(StoredProcedures.UPDATE_IN_PROGRESS_TASK_TO_READY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unable to mark in progress tasks as ready");
            }
        }
    }
}