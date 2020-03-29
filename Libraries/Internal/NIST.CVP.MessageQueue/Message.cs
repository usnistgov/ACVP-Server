using ACVPWorkflow;
using NIST.CVP.MessageQueue.MessagePayloads;
using System;
using System.Text.Json;

namespace NIST.CVP.MessageQueue
{
	public class Message
	{
		public Guid ID { get; set; }
		public MessageType MessageType { get; set; }
		public string Payload { get; set; }
		public APIAction Action { get => GetAction(); }

		private APIAction GetAction()
		{
			//Determine what the detailed action type is, which depends on the message type and payload
			switch (MessageType)
			{
				case MessageType.Registration:
					return APIAction.RegisterTestSession;

				case MessageType.SubmitResults:
					return APIAction.SubmitVectorSetResults;

				case MessageType.Cancel:
					//Whether this is a test session or vector set cancel depends on the vsId, -1 = Test Session, else Vector Set. So need to read the payload to determine what it is
					CancelPayload cancelPayload = JsonSerializer.Deserialize<CancelPayload>(Payload);
					return cancelPayload.VectorSetID == -1 ? APIAction.CancelTestSession : APIAction.CancelVectorSet;

				case MessageType.Request:
					//Parse the payload to get the action and type
					RequestPayload requestPayload = JsonSerializer.Deserialize<RequestPayload>(Payload);

					return (requestPayload.WorkflowItemType, requestPayload.Action) switch
					{
						(WorkflowItemType.Organization, "Create") => APIAction.CreateVendor,
						(WorkflowItemType.Organization, "Update") => APIAction.UpdateVendor,
						(WorkflowItemType.Organization, "Delete") => APIAction.DeleteVendor,
						(WorkflowItemType.Person, "Create") => APIAction.CreatePerson,
						(WorkflowItemType.Person, "Update") => APIAction.UpdatePerson,
						(WorkflowItemType.Person, "Delete") => APIAction.DeletePerson,
						(WorkflowItemType.Implementation, "Create") => APIAction.CreateImplementation,
						(WorkflowItemType.Implementation, "Update") => APIAction.UpdateImplementation,
						(WorkflowItemType.Implementation, "Delete") => APIAction.DeleteImplementation,
						(WorkflowItemType.Dependency, "Create") => APIAction.CreateDependency,
						(WorkflowItemType.Dependency, "Update") => APIAction.UpdateDependency,
						(WorkflowItemType.Dependency, "Delete") => APIAction.DeleteDependency,
						(WorkflowItemType.OE, "Create") => APIAction.CreateOE,
						(WorkflowItemType.OE, "Update") => APIAction.UpdateOE,
						(WorkflowItemType.OE, "Delete") => APIAction.DeleteOE,
						(WorkflowItemType.Validation, _) => APIAction.CertifyTestSession,
						_ => APIAction.Unknown,
					};

				case MessageType.Event:			//Because these seem to be garbage messages, can't find anything the generates them
				default:
					return APIAction.Unknown;
			}

		}
	}
}
