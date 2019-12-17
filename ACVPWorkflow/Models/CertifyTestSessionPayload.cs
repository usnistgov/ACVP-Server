using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACVPWorkflow.Models
{
	public class CertifyTestSessionPayload
	{
		[JsonPropertyName("testSessionId")]
		public long TestSessionID { get; set; }

		[JsonPropertyName("productUrl")]
		public string ImplementationURL { get; set; }

		[JsonPropertyName("oeUrl")]
		public string OEURL { get; set; }

		[JsonPropertyName("certification")]
		public PointlessWrapper ContainerForPrereqs { get; set; }		//TODO kill this thing when we redo the public side, this message is stupid


		public class PointlessWrapper
		{
			[JsonPropertyName("algorithmPrerequisites")]
			public List<AlgorithmPrerequisite> AlgorithmPrerequisites { get; set; }
		}

		public class AlgorithmPrerequisite
		{
			[JsonPropertyName("algorithm")]
			public string Algorithm { get; set; }

			[JsonPropertyName("mode")]
			public string Mode { get; set; }

			[JsonPropertyName("prerequisites")]
			public List<ValidationReference> ValidationReferences { get; set; }
		}

		public class ValidationReference
		{
			[JsonPropertyName("algorithm")]
			public string Algorithm { get; set; }

			[JsonPropertyName("validationId")]
			public string ValidationID { get; set; }
		}
	}


}
