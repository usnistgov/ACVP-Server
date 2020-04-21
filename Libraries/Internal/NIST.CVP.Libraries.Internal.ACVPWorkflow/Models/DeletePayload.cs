using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.Models
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
