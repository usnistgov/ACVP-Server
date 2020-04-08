using System.Collections.Generic;
using System.Text.Json.Serialization;
using ACVPCore.Models.Parameters;

namespace ACVPWorkflow.Models
{
	public class CertifyTestSessionPayload : BasePayload, IWorkflowItemPayload
	{
		[JsonPropertyName("testSessionId")]
		public long TestSessionID { get; set; }

		[JsonPropertyName("productUrl")]
		public string ImplementationURL { get; set; }

		[JsonPropertyName("product")]
		public ImplementationCreatePayload ImplementationToCreate { get; set; }

		[JsonPropertyName("oeUrl")]
		public string OEURL { get; set; }

		[JsonPropertyName("oe")]
		public OECreatePayload OEToCreate { get; set; }

		[JsonPropertyName("certification")]
		public PointlessWrapper ContainerForPrereqs { get; set; }       //TODO kill this thing when we redo the public side, this message is stupid


		public CertifyTestSessionParameters ToCertifyTestSessionParameters()
		{
			CertifyTestSessionParameters certifyTestSessionParameters = new CertifyTestSessionParameters
			{
				TestSessionID = TestSessionID,
				ImplementationID = ParseNullableIDFromURL(ImplementationURL),
				OEID = ParseNullableIDFromURL(OEURL),
				Prerequisites = new List<CertifyTestSessionParameters.AlgorithmPrerequisites>()
			};

			//Translate the prereqs. This is kind of a mess because the prereqs as implemented in the Java public stuff doesn't make much sense. It only allows things like C:123, A:456, and AES:789 as the "validationId"
			if (ContainerForPrereqs != null)
			{
				foreach (var payloadAlgorithmPrerequisites in ContainerForPrereqs.AlgorithmPrerequisites)
				{
					var algorithmPrerequisites = new CertifyTestSessionParameters.AlgorithmPrerequisites
					{
						AlgorithmName = payloadAlgorithmPrerequisites.Algorithm,
						AlgorithmMode = payloadAlgorithmPrerequisites.Mode,
						ValidationReferences = new List<CertifyTestSessionParameters.ValidationReference>()
					};

					foreach (var validationReference in payloadAlgorithmPrerequisites.ValidationReferences)
					{
						var splitValidationID = validationReference.ValidationID.Split(":");

						algorithmPrerequisites.ValidationReferences.Add(new CertifyTestSessionParameters.ValidationReference
						{
							AlgorithmFamily = validationReference.Algorithm,
							ValidationSource = splitValidationID[0],
							ValidationNumber = long.Parse(splitValidationID[1])
						});
					}

					//Finally can add 
					certifyTestSessionParameters.Prerequisites.Add(algorithmPrerequisites);
				}
			}

			return certifyTestSessionParameters;
		}


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
