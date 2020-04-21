using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Internal.ACVPCore.Models;
using NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters;
using NIST.CVP.Libraries.Shared.Algorithms.External;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
{
	public interface IValidationService
	{
		InsertResult Create(ValidationSource validationSource, long implementationID);
		InsertResult CreateScenario(long validationID, long oeID);
		long GetLatestACVPValidationForImplementation(long implementationID);
		long GetScenarioIDForValidationOE(long validationID, long oeID);
		List<(long ValidationID, int ValidationSource)> GetValidationsForImplementation(long implementationID);
		List<(long ScenarioAlgorithmID, long AlgorithmID)> GetScenarioAlgorithms(long scenarioID);
		void DeleteScenarioAlgorithm(long scenarioAlgorithmID);
		long GetValidationNumber(ValidationSource validationSource);
		InsertResult AddScenarioAlgorithm(long scenarioID, long algorithmID);
		void CreateCapabilities(long algorithmID, long scenarioAlgorithmID, IExternalAlgorithm externalAlgorithm);
		Result LogValidationTestSession(long validationID, long testSessionID);
		PagedEnumerable<ValidationLite> GetValidations(ValidationListParameters param);
		Validation GetValidation(long validationId);
	}
}