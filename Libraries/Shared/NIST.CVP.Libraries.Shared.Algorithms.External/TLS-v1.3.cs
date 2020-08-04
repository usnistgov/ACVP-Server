using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class TLS_v1_3 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("hmacAlg")]
		public List<string> HmacAlgorithms { get; set; }
		
		[JsonPropertyName("runningMode")]
		public List<string> RunningMode { get; set; }

		public TLS_v1_3()
		{
			Name = "TLS-v1.3";
			Mode = "KDF";
			Revision = "RFC8446";
		}
	}
}
