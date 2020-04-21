using System.Text.Json.Serialization;

namespace NIST.CVP.Libraries.Internal.TaskQueue
{
	public class GenerationTask
	{
		[JsonPropertyName("isSample")]
		public bool IsSample { get; set; }

		[JsonPropertyName("vsId")]
		public long VectorSetID { get; set; }
	}
}
