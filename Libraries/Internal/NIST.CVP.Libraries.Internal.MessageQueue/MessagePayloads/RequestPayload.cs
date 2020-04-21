using System.Text.Json;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Internal.ACVPWorkflow;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;

namespace NIST.CVP.Libraries.Internal.MessageQueue.MessagePayloads
{
	public class RequestPayload
	{
		[JsonPropertyName("id")]
		public long RequestID { get; set; }

		[JsonPropertyName("action")]
		public string Action { get; set; }

		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("userId")]
		public long UserID { get; set; }

		[JsonPropertyName("json")]
		public JsonElement Json { get; set; }

		public WorkflowItemType WorkflowItemType
		{
			get => Type switch
			{
				"Organizations" => WorkflowItemType.Organization,
				"Persons" => WorkflowItemType.Person,
				"Modules" => WorkflowItemType.Implementation,
				"Dependencies" => WorkflowItemType.Dependency,
				"Oes" => WorkflowItemType.OE,
				"Validations" => WorkflowItemType.Validation,
				_ => WorkflowItemType.Unknown
			};
		}
	}

	public class DeletePayload
	{
		[JsonPropertyName("id")]
		public long ID { get; set; }
	}
}
