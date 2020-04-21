using NIST.CVP.Libraries.Internal.MessageQueue;
using NIST.CVP.Libraries.Shared.Results;

namespace MessageQueueProcessor.MessageProcessors
{
	public interface IMessageProcessor
	{
		public Result Process(Message message);
	}
}
