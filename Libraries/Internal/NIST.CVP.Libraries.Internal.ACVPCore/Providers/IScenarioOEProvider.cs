using NIST.CVP.Libraries.Shared.Results;


namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
{
	public interface IScenarioOEProvider
	{
		Result Insert(long scenarioID, long oeID);
	}
}