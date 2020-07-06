using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class KAS_KDF_TwoStep_SP800_56Cr1 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("l")]
		public int L { get; set; }

		[JsonPropertyName("z")]
		public Domain Z { get; set; }

		[JsonPropertyName("capabilities")]
		public List<TwoStepCapability> Capabilities { get; set; }
		
		public KAS_KDF_TwoStep_SP800_56Cr1()
		{
			Name = "KAS-KDF";
			Mode = "TwoStep";
			Revision = "Sp800-56Cr1";
		}
		
		public class TwoStepCapability
		{
			[JsonPropertyName("macSaltMethods")]
			public List<string> MacSaltMethods { get; set; }
			
			[JsonPropertyName("fixedInfoPattern")]
			public string FixedInfoPattern { get; set; }
			
			[JsonPropertyName("encoding")]
			public List<string> Encoding { get; set; }
			
			[JsonPropertyName("kdfMode")]
			public string KDFMode { get; set; }

			[JsonPropertyName("macMode")]
			public List<string> MacMode { get; set; }

			[JsonPropertyName("supportedLengths")]
			public Domain SupportedLengths { get; set; }

			[JsonPropertyName("fixedDataOrder")]
			public List<string> FixedDataOrder { get; set; }

			[JsonPropertyName("counterLength")]
			public List<int> CounterLength { get; set; }

			[JsonPropertyName("supportsEmptyIv")]
			public bool SupportsEmptyIV { get; set; }
			
			[JsonPropertyName("requiresEmptyIv")]
			public bool RequiresEmptyIV { get; set; }
		}
	}
}
