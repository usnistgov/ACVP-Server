using NIST.CVP.Libraries.Internal.ACVPWorkflow;
using MessageQueueProcessor.MessageProcessors;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;

namespace MessageQueueProcessor
{
	public interface IMessageProcessorFactory
	{
		public IMessageProcessor GetMessageProcessor(APIAction action);
	}
}