using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class KAS_KDF_HKDF_SP800_56Cr1 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("l")]
		public int L { get; set; }

		[JsonPropertyName("z")]
		public Domain Z { get; set; }
		
		[JsonPropertyName("hmacAlg")]
		public List<string> HmacAlg { get; set; }
		
		[JsonPropertyName("fixedInfoPattern")]
		public string FixedInfoPattern { get; set; }

		[JsonPropertyName("encoding")]
		public List<string> Encoding { get; set; }
		
		public KAS_KDF_HKDF_SP800_56Cr1()
		{
			Name = "KAS-KDF";
			Mode = "HKDF";
			Revision = "Sp800-56Cr1";
		}
	}
}
