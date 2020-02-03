using System.Data;
using NIST.CVP.TaskQueueProcessor.Providers;

namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public interface ITaskRetriever
    {
        ExecutableTask GetTaskFromRow(IDataReader reader, IDbProvider dbProvider);
    }
}