using ACVPCore.Models.Parameters;

namespace ACVPWorkflow.Models
{
	public class DeletePayload
	{
		public long ID { get; set; }

		public DeleteParameters ToDeleteParameters() => new DeleteParameters
		{
			ID = ID
		};
	}
}
