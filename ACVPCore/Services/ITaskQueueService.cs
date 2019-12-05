namespace ACVPCore.Services
{
	public interface ITaskQueueService
	{
		void AddGenerationTask(GenerationTask task);
	}
}