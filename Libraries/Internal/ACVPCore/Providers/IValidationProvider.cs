using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using NIST.CVP.Enumerables;
using NIST.CVP.Results;


namespace ACVPCore.Providers
{
	public interface IValidationProvider
	{
		List<(long ValidationID, int ValidationSource)> GetValidationsForImplementation(long implementationID);
		InsertResult Insert(ValidationSource validationSource, long validationNumber, long implementationID);
		long GetNextACVPValidationNumber();
		long GetNextLCAVPValidationNumber();
		Result ValidationTestSessionInsert(long validationID, long testSessionID);
		PagedEnumerable<ValidationLite> GetValidations(ValidationListParameters param);
		Validation GetValidation(long validationId);
	}
}