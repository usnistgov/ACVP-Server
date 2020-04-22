using NIST.CVP.Libraries.Shared.Results;


namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
{
	public interface IPrerequisiteProvider
	{
		Result DeleteAllForScenarioAlgorithm(long scenarioAlgorithmID);
		InsertResult Insert(long scenarioAlgorithmID, long validationID, string requirement);
	}
}