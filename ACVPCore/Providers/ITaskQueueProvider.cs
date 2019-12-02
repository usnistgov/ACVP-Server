namespace ACVPCore.Providers
{
	public interface ITaskQueueProvider
	{
		void Insert(TaskType type, string payload);
	}
}