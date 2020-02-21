using ACVPCore.Results;

namespace ACVPCore.Providers
{
	public interface IPrerequisiteProvider
	{
		Result DeleteAllForScenarioAlgorithm(long scenarioAlgorithmID);
	}
}