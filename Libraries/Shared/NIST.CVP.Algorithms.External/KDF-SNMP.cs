using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Algorithms.DataTypes;

namespace NIST.CVP.Algorithms.External
{
	public class KDF_SNMP : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("passwordLength")]
		public Domain PasswordLength { get; set; }

		[JsonPropertyName("engineId")]
		public List<string> EngineIDs { get; set; }

		public KDF_SNMP()
		{
			Name = "kdf-components";
			Mode = "snmp";
			Revision = "1.0";
		}
	}
}
