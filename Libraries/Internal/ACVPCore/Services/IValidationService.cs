using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using NIST.CVP.Algorithms.External;
using NIST.CVP.Enumerables;
using NIST.CVP.Results;

namespace ACVPCore.Services
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