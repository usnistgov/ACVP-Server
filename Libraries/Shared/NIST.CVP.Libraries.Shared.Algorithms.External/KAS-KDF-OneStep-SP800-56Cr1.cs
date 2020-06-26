using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class KAS_KDF_OneStep_SP800_56Cr1 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("l")]
		public int L { get; set; }

		[JsonPropertyName("auxFunctions")]
		public List<AuxFunction> AuxFunctions { get; set; }
		
		[JsonPropertyName("fixedInfoPattern")]
		public string FixedInfoPattern { get; set; }

		[JsonPropertyName("encoding")]
		public List<string> Encoding { get; set; }
		
		public KAS_KDF_OneStep_SP800_56Cr1()
		{
			Name = "KAS-KDF";
			Mode = "OneStep";
			Revision = "Sp800-56Cr1";
		}
		
		public class AuxFunction
		{
			[JsonPropertyName("auxFunctionName")]
			public string AuxFunctionName { get; set; }

			[JsonPropertyName("macSaltMethods")]
			public List<string> MacSaltMethods { get; set; }
		}
	}
}
