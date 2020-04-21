using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class KDF_SSH : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("cipher")]
		public List<string> Ciphers { get; set; }

		[JsonPropertyName("hashAlg")]
		public List<string> HashAlgorithms { get; set; }

		public KDF_SSH()
		{
			Name = "kdf-components";
			Mode = "ssh";
			Revision = "1.0";
		}
	}
}
