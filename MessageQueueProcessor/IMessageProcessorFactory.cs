using MessageQueueProcessor.MessageProcessors;

namespace MessageQueueProcessor
{
	public interface IMessageProcessorFactory
	{
		public IMessageProcessor GetMessageProcessor(MessageAction action);
	}
}