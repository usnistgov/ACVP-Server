using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
{
	public interface ICapabilityProvider
	{
		InsertResult Insert(long validationOEAlgorithmID, long propertyID, long? parentCapabilityID, int? orderIndex, bool historical, string stringValue, long? numberValue, bool? booleanValue);
		Result DeleteAllForValidationOEAlgorithm(long validationOEAlgorithmID);
	}
}