using System.Collections.Generic;
using ACVPCore.Models.ProtoModels;
using ACVPCore.Results;

namespace ACVPCore.Services
{
	public interface IValidationService
	{
		InsertResult Create(long implementationID, bool isLCAVP = false);
		long GetLatestACVPValidationForImplementation(long implementationID);
		List<(long ValidationID, int ValidationSource)> GetValidationsForImplementation(long implementationID);
		long GetValidationNumber(ValidationSource validationSource);
		long FindMatchingScenario(long validationID, List<ScenarioAlgorithm> scenarioAlgorithms);
		InsertResult AddScenarioToValidation(long validationID);
		Result AddOEToScenario(long scenarioID, long oeID);


	}
}