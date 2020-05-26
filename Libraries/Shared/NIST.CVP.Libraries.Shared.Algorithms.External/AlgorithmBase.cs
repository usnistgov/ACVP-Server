using System.Text;
using System.Text.Json.Serialization;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class AlgorithmBase
	{
		[JsonPropertyName("id")]
		public int AlgorithmId { get; set; }
		
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("mode")]
		public string Mode { get; set; }

		[JsonPropertyName("revision")]
		public string Revision { get; set; }

		[JsonIgnore]
		public string FullAlgoName
		{
			get
			{
				var separator = "-";
				var sb = new StringBuilder();
				sb.Append(Name);
				sb.Append(separator);
				if (!string.IsNullOrEmpty(Mode))
				{
					sb.Append(Mode);
					sb.Append(separator);
				}

				sb.Append(Revision);

				return sb.ToString();
			}
		}
	}
}
