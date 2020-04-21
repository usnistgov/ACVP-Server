using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Internal.ACVPCore.Models;
using NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters;
using NIST.CVP.Libraries.Shared.Results;


namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
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