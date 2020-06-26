using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Internal.ACVPCore.Models;
using NIST.CVP.Libraries.Internal.ACVPCore.Providers;
using NIST.CVP.Libraries.Internal.Algorithms.Persisted;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.Algorithms.External;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
{
	public class ValidationService : IValidationService
	{
		private readonly IValidationProvider _validationProvider;
		private readonly IPrerequisiteService _prerequisiteService;
		private readonly IValidationOEAlgorithmProvider _validationOEAlgorithmProvider;
		private readonly ICapabilityService _capabilityService;

		public ValidationService(IValidationProvider validationProvider, IPrerequisiteService prerequisiteService, IValidationOEAlgorithmProvider validationOEAlgorithmProvider, ICapabilityService capabilityService)
		{
			_validationProvider = validationProvider;
			_prerequisiteService = prerequisiteService;
			_validationOEAlgorithmProvider = validationOEAlgorithmProvider;
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

		public InsertResult AddValidationOEAlgorithm(long validationID, long oeID, long algorithmID, long vectorSetID) => _validationOEAlgorithmProvider.Insert(validationID, oeID, algorithmID, vectorSetID);

		public void InactivateValidationOEAlgorithm(long validationOEAlgorithmID)
		{
			//Delete the capabilities
			Result capabilitiesDeleteResult = _capabilityService.DeleteAllForValidationOEAlgorithm(validationOEAlgorithmID);

			//Delete the prereqs
			Result prereqsDeleteResult = _prerequisiteService.DeleteAllForValidationOEAlgorithm(validationOEAlgorithmID);

			//Inactivate the validation OE algorithm
			Result validationOEAlgorithmInactivateResult = _validationOEAlgorithmProvider.Inactivate(validationOEAlgorithmID);
		}

		public List<(long ValidationOEAlgorithmID, long AlgorithmID)> GetValidationOEAlgorithms(long validationID, long oeID) => _validationOEAlgorithmProvider.GetActiveValidationOEAlgorithms(validationID, oeID);
		public List<ValidationOEAlgorithmDisplay> GetActiveValidationOEAlgorithmsForDisplay(long validationID) => _validationOEAlgorithmProvider.GetActiveValidationOEAlgorithmsForDisplay(validationID);

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
		public void CreateCapabilities(long validationOEAlgorithmID, long algorithmID, IExternalAlgorithm externalAlgorithm)
		{
			//Convert it to a persistence algorithm
			IPersistedAlgorithm persistenceAlgorithm = PersistedAlgorithmFactory.GetPersistedAlgorithm(externalAlgorithm);

			//Persist it - the entire algorithm object is just a class as far as the persistence mechanism is concerned, just with some non-property properties on it
			_capabilityService.CreateClassCapabilities(algorithmID, validationOEAlgorithmID, null, 0, null, persistenceAlgorithm);
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
