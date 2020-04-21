using NIST.CVP.Libraries.Internal.ACVPCore.Providers;
using NIST.CVP.Libraries.Shared.Results;


namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
{
	public class PrerequisiteService : IPrerequisiteService
	{
		private readonly IPrerequisiteProvider _prerequisiteProvider;

		public PrerequisiteService(IPrerequisiteProvider PrerequisiteProvider)
		{
			_prerequisiteProvider = PrerequisiteProvider;
		}

		public Result DeleteAllForScenarioAlgorithm(long scenarioAlgorithmID) => _prerequisiteProvider.DeleteAllForScenarioAlgorithm(scenarioAlgorithmID);

		public InsertResult Create(long scenarioAlgorithmID, long validationID, string requirement) => _prerequisiteProvider.Insert(scenarioAlgorithmID, validationID, requirement);
	}
}
