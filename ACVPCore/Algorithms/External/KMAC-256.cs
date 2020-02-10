using System.Collections.Generic;
using System.Text.Json.Serialization;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.External
{
	public class KMAC_256 : AlgorithmBase, IExternalAlgorithm
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

		public KMAC_256()
		{
			Name = "KMAC-256";
			Revision = "1.0";
		}
	}
}
