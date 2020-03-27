using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Algorithms.DataTypes;

namespace NIST.CVP.Algorithms.External
{
	public class KMAC_128 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("digestSize")]
		public List<int> DigestSize { get; set; }

		[JsonPropertyName("msgLen")]
		public List<Range> MessageLength { get; set; }

		[JsonPropertyName("macLen")]
		public List<Range> MacLength { get; set; }

		[JsonPropertyName("keyLen")]
		public List<Range> KeyLength { get; set; }

		[JsonPropertyName("hexCustomization")]
		public bool HexCustomization { get; set; }

		[JsonPropertyName("xof")]
		public List<bool> XOF { get; set; }

		public KMAC_128()
		{
			Name = "KMAC-128";
			Revision = "1.0";
		}
	}
}
