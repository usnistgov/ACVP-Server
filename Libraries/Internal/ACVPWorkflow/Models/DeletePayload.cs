using System.Text.Json.Serialization;
using ACVPCore.Models.Parameters;

namespace ACVPWorkflow.Models
{
	public class DeletePayload : IWorkflowItemPayload
	{
		[JsonPropertyName("id")]
		public long ID { get; set; }

		public DeleteParameters ToDeleteParameters() => new DeleteParameters
		{
			ID = ID
		};
	}
}
