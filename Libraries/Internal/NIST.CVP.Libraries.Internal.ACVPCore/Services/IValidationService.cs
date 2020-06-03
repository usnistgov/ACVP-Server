using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.Algorithms.External;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
{
	public interface IValidationService
	{
		InsertResult Create(ValidationSource validationSource, long implementationID);
		long GetLatestACVPValidationForImplementation(long implementationID);
		List<(long ValidationID, int ValidationSource)> GetValidationsForImplementation(long implementationID);
		List<(long ValidationOEAlgorithmID, long AlgorithmID)> GetValidationOEAlgorithms(long validationID, long oeID);
		void InactivateValidationOEAlgorithm(long validationOEAlgorithmID);
		long GetValidationNumber(ValidationSource validationSource);
		InsertResult AddValidationOEAlgorithm(long validationID, long oeID, long algorithmID, long vectorSetID);
		void CreateCapabilities(long validationOEAlgorithmID, long algorithmID, IExternalAlgorithm externalAlgorithm);
		Result LogValidationTestSession(long validationID, long testSessionID);
		PagedEnumerable<ValidationLite> GetValidations(ValidationListParameters param);
		Validation GetValidation(long validationId);
	}
}