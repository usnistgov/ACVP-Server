using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Results;

namespace ACVPCore.Providers
{
	public interface IValidationProvider
	{
		List<(long ValidationID, int ValidationSource)> GetValidationsForImplementation(long implementationID);
		InsertResult Insert(ValidationSource validationSource, long validationNumber, long implementationID);
		long GetNextACVPValidationNumber();
		long GetNextLCAVPValidationNumber();
		Result ValidationTestSessionInsert(long validationID, long testSessionID);
		List<ValidationLite> GetValidations();
		Validation GetValidation(long validationId);
	}
}