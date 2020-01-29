using System.Collections.Generic;
using ACVPCore.Results;

namespace ACVPCore.Providers
{
	public interface IScenarioAlgorithmProvider
	{
		InsertResult Insert(long scenarioID, long algorithmID);
		Result Delete(long scenarioAlgorithmID);
		List<(long ScenarioAlgorithmID, long AlgorithmID)> GetScenarioAlgorithmsForScenario(long scenarioID);
	}
}