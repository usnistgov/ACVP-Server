using ACVPWorkflow;
using MessageQueueProcessor.MessagePayloads;
using System;
using System.Text.Json;

namespace MessageQueueProcessor
{
	public class Message
	{
		public Guid ID { get; set; }
		public MessageType MessageType { get; set; }
		public string Payload { get; set; }
		public MessageAction Action { get => GetAction(); }

		private MessageAction GetAction()
		{
			//Determine what the detailed action type is, which depends on the message type and payload
			switch (MessageType)
			{
				case MessageType.Registration:
					return MessageAction.RegisterTestSession;

				case MessageType.SubmitResults:
					return MessageAction.SubmitVectorSetResults;

				case MessageType.Cancel:
					//Whether this is a test session or vector set cancel depends on the vsId, -1 = Test Session, else Vector Set. So need to read the payload to determine what it is
					CancelPayload cancelPayload = JsonSerializer.Deserialize<CancelPayload>(Payload);
					return cancelPayload.VectorSetID == -1 ? MessageAction.CancelTestSession : MessageAction.CancelVectorSet;

				case MessageType.Request:
					//Parse the payload to get the action and type
					RequestPayload requestPayload = JsonSerializer.Deserialize<RequestPayload>(Payload);

					return requestPayload.WorkflowItemType switch
					{
						WorkflowItemType.Organization => requestPayload.Action switch
						{
							"Create" => MessageAction.CreateVendor,
							"Update" => MessageAction.UpdateVendor,
							"Delete" => MessageAction.DeleteVendor,
							_ => MessageAction.Unknown,
						},

						WorkflowItemType.Person => requestPayload.Action switch
						{
							"Create" => MessageAction.CreatePerson,
							"Update" => MessageAction.UpdatePerson,
							"Delete" => MessageAction.DeletePerson,
							_ => MessageAction.Unknown,
						},

						WorkflowItemType.Implementation => requestPayload.Action switch
						{
							"Create" => MessageAction.CreateImplementation,
							"Update" => MessageAction.UpdateImplementation,
							"Delete" => MessageAction.DeleteImplementation,
							_ => MessageAction.Unknown,
						},

						WorkflowItemType.Dependency => requestPayload.Action switch
						{
							"Create" => MessageAction.CreateDependency,
							"Update" => MessageAction.UpdateDependency,
							"Delete" => MessageAction.DeleteDependency,
							_ => MessageAction.Unknown,
						},

						WorkflowItemType.OE => requestPayload.Action switch
						{
							"Create" => MessageAction.CreateOE,
							"Update" => MessageAction.UpdateOE,
							"Delete" => MessageAction.DeleteOE,
							_ => MessageAction.Unknown,
						},

						WorkflowItemType.Validation => MessageAction.CertifyTestSession,

						_ => MessageAction.Unknown,
					};
				case MessageType.Event:			//Because these seem to be garbage messages, can't find anything the generates them
				default:
					return MessageAction.Unknown;
			}

		}
	}
}
