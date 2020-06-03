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

		public Result DeleteAllForValidationOEAlgorithm(long validationOEAlgorithm) => _prerequisiteProvider.DeleteAllForValidationOEAlgorithm(validationOEAlgorithm);

		public InsertResult Create(long validationOEAlgorithmID, long validationID, string requirement) => _prerequisiteProvider.Insert(validationOEAlgorithmID, validationID, requirement);
	}
}
