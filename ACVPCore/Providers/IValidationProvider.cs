using ACVPCore.Results;

namespace ACVPCore.Providers
{
	public interface IValidationProvider
	{
		System.Collections.Generic.List<(long ValidationID, int ValidationSource)> GetValidationsForImplementation(long implementationID);
		InsertResult Insert(long implementationID, bool isLCAVP = false);
	}
}