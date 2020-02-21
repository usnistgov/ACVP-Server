using ACVPCore.Results;

namespace ACVPCore.Services
{
	public interface IPrerequisiteService
	{
		Result DeleteAllForScenarioAlgorithm(long scenarioAlgorithmID);
	}
}