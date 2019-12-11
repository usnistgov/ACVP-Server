using System.Data;

namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public interface ITaskRetriever
    {
        ExecutableTask GetTaskFromRow(IDataReader reader);
    }
}