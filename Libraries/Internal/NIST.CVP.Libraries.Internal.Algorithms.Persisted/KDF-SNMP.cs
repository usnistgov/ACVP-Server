using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class KDF_SNMP : PersistedAlgorithmBase
	{
		[AlgorithmProperty("passwordLength")]
		public Domain PasswordLength { get; set; }

		[AlgorithmProperty("engineId")]
		public List<string> EngineIDs { get; set; }

		public KDF_SNMP()
		{
			Name = "kdf-components";
			Mode = "snmp";
			Revision = "1.0";
		}

		public KDF_SNMP(External.KDF_SNMP external) : this()
		{
			PasswordLength = external.PasswordLength;
			EngineIDs = external.EngineIDs;
		}
	}

	
}
