using ACVPCore.Results;

namespace ACVPCore.Services
{
	public interface IValidationService
	{
		InsertResult Create(long implementationID, bool isLCAVP = false);
		long GetLatestACVPValidationForImplementation(long implementationID);
		System.Collections.Generic.List<(long ValidationID, int ValidationSource)> GetValidationsForImplementation(long implementationID);
	}
}