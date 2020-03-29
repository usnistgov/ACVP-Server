using NIST.CVP.MessageQueue;
using NIST.CVP.Results;

namespace MessageQueueProcessor.MessageProcessors
{
	public interface IMessageProcessor
	{
		public Result Process(Message message);
	}
}
