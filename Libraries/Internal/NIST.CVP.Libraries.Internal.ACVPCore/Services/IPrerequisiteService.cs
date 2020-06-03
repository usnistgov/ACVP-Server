using NIST.CVP.Libraries.Shared.Results;


namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
{
	public interface IPrerequisiteService
	{
		Result DeleteAllForValidationOEAlgorithm(long validationOEAlgorithm);
		InsertResult Create(long validationOEAlgorithmID, long validationID, string requirement);
	}
}