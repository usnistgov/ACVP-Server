using NIST.CVP.Libraries.Shared.Results;


namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
{
	public interface IPrerequisiteService
	{
		Result DeleteAllForScenarioAlgorithm(long scenarioAlgorithmID);
		InsertResult Create(long scenarioAlgorithmID, long validationID, string requirement);
	}
}