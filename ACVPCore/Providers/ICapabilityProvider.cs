using System.Collections.Generic;
using ACVPCore.Algorithms.Persisted;
using ACVPCore.Results;

namespace ACVPCore.Providers
{
	public interface ICapabilityProvider
	{
		InsertResult Insert(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, AlgorithmPropertyType type, int? orderIndex, bool historical, string stringValue, long? numberValue, bool? booleanValue);
		Result DeleteAllForScenarioAlgorithm(long scenarioAlgorithmID);
	}
}