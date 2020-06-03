using NIST.CVP.Libraries.Shared.Results;


namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
{
	public interface IPrerequisiteProvider
	{
		Result DeleteAllForValidationOEAlgorithm(long validationOEAlgorithm);
		InsertResult Insert(long validationOEAlgorithmID, long validationID, string requirement);
	}
}