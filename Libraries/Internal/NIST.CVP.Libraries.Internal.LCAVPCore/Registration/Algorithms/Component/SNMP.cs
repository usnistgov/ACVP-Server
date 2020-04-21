using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.Algorithms.Persisted;
using Newtonsoft.Json;
using NIST.CVP.Libraries.Internal.LCAVPCore.Registration.MathDomain;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration.Algorithms.Component
{
	public class SNMP : AlgorithmBase, IAlgorithm
	{
		[JsonProperty("engineId")]
		public List<string> EngineIDs { get; private set; } = new List<string>();

		[JsonProperty("passwordLength")]
		public Domain PasswordLength { get; private set; } = new Domain();

		public SNMP(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "kdf-components", "snmp")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("KDF_800_135_Prerequisite_SHA")));

			EngineIDs.Add(options.GetValue("KDF_800_135_snmpEngineId[0]"));
			EngineIDs.Add(options.GetValue("KDF_800_135_snmpEngineId[1]"));

			int minValue = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("KDF_800_135_snmpPasswordLength[0]"));
			int maxValue = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("KDF_800_135_snmpPasswordLength[1]"));

			if (minValue == maxValue)
			{
				PasswordLength.Add(minValue);
			}
			else {
				PasswordLength.Add(new Range
				{
					Min = minValue,
					Max = maxValue
				});
			}
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Libraries.Internal.Algorithms.Persisted.KDF_SNMP
		{
			EngineIDs = EngineIDs,
			PasswordLength = PasswordLength.ToCoreDomain()
		};
	}
}
