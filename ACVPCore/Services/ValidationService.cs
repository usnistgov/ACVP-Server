using System.Collections.Generic;
using System.Linq;
using ACVPCore.Algorithms;
using ACVPCore.Algorithms.External;
using ACVPCore.Algorithms.Persisted;
using ACVPCore.Providers;
using ACVPCore.Results;

namespace ACVPCore.Services
{
	public class ValidationService : IValidationService
	{
		private readonly IValidationProvider _validationProvider;
		private readonly IScenarioProvider _scenarioProvider;
		private readonly IScenarioOEProvider _scenarioOEProvider;
		private readonly IScenarioAlgorithmProvider _scenarioAlgorithmProvider;
		private readonly ICapabilityProvider _capabilityProvider;
		private readonly ICapabilityService _capabilityService;

		public ValidationService(IValidationProvider validationProvider, IScenarioProvider scenarioProvider, IScenarioOEProvider scenarioOEProvider, IScenarioAlgorithmProvider scenarioAlgorithmProvider, ICapabilityProvider capabilityProvider, ICapabilityService capabilityService)
		{
			_validationProvider = validationProvider;
			_scenarioProvider = scenarioProvider;
			_scenarioOEProvider = scenarioOEProvider;
			_scenarioAlgorithmProvider = scenarioAlgorithmProvider;
			_capabilityProvider = capabilityProvider;
			_capabilityService = capabilityService;
		}

		public InsertResult Create(ValidationSource validationSource, long implementationID)
		{
			//Get the validation number to assign to it
			long validationNumber = GetValidationNumber(validationSource);

			if (validationNumber == -1) return new InsertResult("Failed to get new validation number");

			//Create the validation record
			InsertResult validationInsertResult = _validationProvider.Insert(validationSource, validationNumber, implementationID);

			return validationInsertResult;
		}

		public InsertResult CreateScenario(long validationID, long oeID)
		{
			//Create the scenario record
			InsertResult scenarioCreateResult = AddScenarioToValidation(validationID);

			if (!scenarioCreateResult.IsSuccess) return scenarioCreateResult;

			long scenarioID = scenarioCreateResult.ID;

			//Create the scenario OE link
			Result OEResult = AddOEToScenario(scenarioID, oeID);

			return OEResult.IsSuccess ? scenarioCreateResult : (InsertResult)OEResult;
		}

		public long GetScenarioIDForValidationOE(long validationID, long oeID) => _scenarioProvider.GetScenarioIDForValidationOE(validationID, oeID);

		private InsertResult AddScenarioToValidation(long validationID) => _scenarioProvider.Insert(validationID);

		private Result AddOEToScenario(long scenarioID, long oeID) => _scenarioOEProvider.Insert(scenarioID, oeID);

		public InsertResult AddScenarioAlgorithm(long scenarioID, long algorithmID) => _scenarioAlgorithmProvider.Insert(scenarioID, algorithmID);

		public void DeleteScenarioAlgorithm(long scenarioAlgorithmID)
		{
			//Delete the capabilities
			Result capabilitiesDeleteResult = _capabilityProvider.DeleteAllForScenarioAlgorithm(scenarioAlgorithmID);

			//Delete the prereqs
			//TODO - prereqs!

			//Delete the scenario algorithm
			Result scenarioAlgorithmDeleteResult = _scenarioAlgorithmProvider.Delete(scenarioAlgorithmID);
		}

		public List<(long ScenarioAlgorithmID, long AlgorithmID)> GetScenarioAlgorithms(long scenarioID) => _scenarioAlgorithmProvider.GetScenarioAlgorithmsForScenario(scenarioID);

		public List<(long ValidationID, int ValidationSource)> GetValidationsForImplementation(long implementationID) => _validationProvider.GetValidationsForImplementation(implementationID);

		//Get the most recently created ACVP validation ID, or 0 if none exists
		public long GetLatestACVPValidationForImplementation(long implementationID) => GetValidationsForImplementation(implementationID).Where(x => x.ValidationSource == 1).DefaultIfEmpty().Max(x => x.ValidationID);

		public long GetValidationNumber(ValidationSource validationSource) => validationSource switch
		{
			ValidationSource.ACVP => _validationProvider.GetNextACVPValidationNumber(),
			ValidationSource.LCAVP => _validationProvider.GetNextLCAVPValidationNumber(),
			_ => -1
		};

		public Result LogValidationTestSession(long validationID, long testSessionID) => _validationProvider.ValidationTestSessionInsert(validationID, testSessionID);

		public void CreateCapabilities(long algorithmID, long scenarioAlgorithmID, IExternalAlgorithm externalAlgorithm)
		{
			//Convert it to a persistence algorithm
			IPersistedAlgorithm persistenceAlgorithm = PersistedAlgorithmFactory.GetPersistedAlgorithm(externalAlgorithm);

			//Persist it - the entire algorithm object is just a class as far as the persistence mechanism is concerned, just with some non-property properties on it
			_capabilityService.CreateClassCapabilities(algorithmID, scenarioAlgorithmID, null, null, 0, 0, persistenceAlgorithm);
		}
	}
}
