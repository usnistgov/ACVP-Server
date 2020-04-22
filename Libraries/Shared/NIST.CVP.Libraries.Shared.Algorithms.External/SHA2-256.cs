using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class SHA2_256 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("digestSize")]
		public List<string> DigestSize { get; set; }

		[JsonPropertyName("messageLength")]
		public Domain MessageLength { get; set; }

		[JsonPropertyName("function")]
		public List<string> Function { get; set; }

		public SHA2_256()
		{
			Name = "SHA2-256";
			Revision = "1.0";
		}
	}
}
