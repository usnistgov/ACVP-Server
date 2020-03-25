using NIST.CVP.Results;

using NIST.CVP.Algorithms.Persisted;

namespace ACVPCore.Providers
{
	public interface ICapabilityProvider
	{
		InsertResult Insert(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, AlgorithmPropertyType type, int? orderIndex, bool historical, string stringValue, long? numberValue, bool? booleanValue);
		Result DeleteAllForScenarioAlgorithm(long scenarioAlgorithmID);
	}
}