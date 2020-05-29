namespace NIST.CVP.Libraries.Shared.MessageQueue.Abstractions
{
	/// <summary>
	/// Represents a workflow item payload.
	/// All workflow items are originally messages, but not all messages are workflow items.
	/// </summary>
	public interface IWorkflowItemPayload : IMessagePayload
	{
	}
}
