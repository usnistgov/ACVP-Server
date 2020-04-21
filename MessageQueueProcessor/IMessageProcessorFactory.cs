using NIST.CVP.Libraries.Internal.ACVPWorkflow;
using MessageQueueProcessor.MessageProcessors;

namespace MessageQueueProcessor
{
	public interface IMessageProcessorFactory
	{
		public IMessageProcessor GetMessageProcessor(APIAction action);
	}
}