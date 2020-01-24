using System.Collections.Generic;
using ACVPCore.Models.Capabilities;
using ACVPCore.Results;

namespace ACVPCore.Providers
{
	public interface ICapabilityProvider
	{
		InsertResult Insert(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, DatabaseCapabilityType type, int? orderIndex, bool historical, string stringValue, long? numberValue, bool? booleanValue);
		List<RawCapability> GetRawCapabilitiesForScenarioAlgorithm(long scenarioAlgorithmID);
	}
}