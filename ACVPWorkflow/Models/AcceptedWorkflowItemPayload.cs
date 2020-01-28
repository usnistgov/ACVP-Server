using System.Text.Json.Serialization;

namespace ACVPWorkflow.Models
{
	public class AcceptedWorkloadItemPayload
	{

		[JsonPropertyName("url")]
		public string URL { get; set; }
	}
}
