using ACVPCore.Results;

namespace ACVPCore.Providers
{
	public interface ITaskQueueProvider
	{
		Result Insert(TaskType type, string payload);
	}
}