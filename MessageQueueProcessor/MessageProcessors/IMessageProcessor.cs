namespace MessageQueueProcessor.MessageProcessors
{
	public interface IMessageProcessor
	{
		public void Process(Message message);
	}
}
