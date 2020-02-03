using ACVPCore.Results;

namespace ACVPCore.Providers
{
	public interface IScenarioOEProvider
	{
		Result Insert(long scenarioID, long oeID);
	}
}