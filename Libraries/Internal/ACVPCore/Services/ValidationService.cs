﻿using System.Collections.Generic;
using System.Linq;
using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Providers;
using NIST.CVP.Algorithms.External;
using NIST.CVP.Algorithms.Persisted;
using NIST.CVP.Enumerables;
using NIST.CVP.Results;

namespace ACVPCore.Services
{
	public class ValidationService : IValidationService
	{
		private readonly IValidationProvider _validationProvider;
		private readonly IPrerequisiteService _prerequisiteService;
		private readonly IScenarioProvider _scenarioProvider;
		private readonly IScenarioOEProvider _scenarioOEProvider;
		private readonly IScenarioAlgorithmProvider _scenarioAlgorithmProvider;
		private readonly ICapabilityService _capabilityService;

		public ValidationService(IValidationProvider validationProvider, IPrerequisiteService prerequisiteService, IScenarioProvider scenarioProvider, IScenarioOEProvider scenarioOEProvider, IScenarioAlgorithmProvider scenarioAlgorithmProvider, ICapabilityService capabilityService)
		{
			_validationProvider = validationProvider;
			_prerequisiteService = prerequisiteService;
			_scenarioProvider = scenarioProvider;
			_scenarioOEProvider = scenarioOEProvider;
			_scenarioAlgorithmProvider = scenarioAlgorithmProvider;
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
			Result capabilitiesDeleteResult = _capabilityService.DeleteAllForScenarioAlgorithm(scenarioAlgorithmID);

			//Delete the prereqs
			Result prereqsDeleteResult = _prerequisiteService.DeleteAllForScenarioAlgorithm(scenarioAlgorithmID);

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
			_capabilityService.CreateClassCapabilities(algorithmID, scenarioAlgorithmID, null, null, 0, 0, null, persistenceAlgorithm);
		}
		
		public PagedEnumerable<ValidationLite> GetValidations(ValidationListParameters param)
		{
			return _validationProvider.GetValidations(param);
		}

		public Validation GetValidation(long validationId)
		{
			var result = _validationProvider.GetValidation(validationId);

			if (result == null)
				return null;
			
			// TODO may want to more fully hydrate the validation object with additional properties using other services
			// or query them independently from the api
			return result;
		}
	}
}