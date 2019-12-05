using System.Data.SqlClient;

namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public interface ITaskRetriever
    {
        ExecutableTask GetTaskFromRow(SqlDataReader reader);
    }
}