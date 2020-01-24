using System.Collections.Generic;
using ACVPCore.Results;

namespace ACVPCore.Providers
{
	public interface IScenarioAlgorithmProvider
	{
		InsertResult Insert(long scenarioID, long algorithmID);
		List<(long ScenarioAlgorithmID, long AlgorithmID)> GetScenarioAlgorithmsForScenario(long scenarioID);
	}
}