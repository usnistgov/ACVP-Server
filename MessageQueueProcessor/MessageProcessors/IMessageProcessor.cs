using ACVPCore.Results;

namespace MessageQueueProcessor.MessageProcessors
{
	public interface IMessageProcessor
	{
		public Result Process(Message message);
	}
}
