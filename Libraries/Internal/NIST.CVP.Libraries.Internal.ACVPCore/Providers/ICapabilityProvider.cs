using NIST.CVP.Libraries.Shared.Results;

using NIST.CVP.Libraries.Internal.Algorithms.Persisted;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
{
	public interface ICapabilityProvider
	{
		InsertResult Insert(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, AlgorithmPropertyType type, int? orderIndex, bool historical, string stringValue, long? numberValue, bool? booleanValue);
		Result DeleteAllForScenarioAlgorithm(long scenarioAlgorithmID);
	}
}