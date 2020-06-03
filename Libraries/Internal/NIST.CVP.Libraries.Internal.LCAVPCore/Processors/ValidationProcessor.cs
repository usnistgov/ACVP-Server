using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Internal.ACVPCore;
using NIST.CVP.Libraries.Internal.ACVPCore.Providers;
using NIST.CVP.Libraries.Shared.Results;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.Algorithms.Persisted;
using NIST.CVP.Libraries.Internal.LCAVPCore.Registration;
using NIST.CVP.Libraries.Internal.LCAVPCore.Registration.Algorithms;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Processors
{
	public class ValidationProcessor : IValidationProcessor
	{
		private readonly IModuleProcessor _moduleProcessor;
		private readonly IOEProcessor _oeProcessor;
		private readonly IValidationService _validationService;
		private readonly IScenarioOEProvider _scenarioOEProvider;
		private readonly ICapabilityService _capabilityService;
		private readonly IPrerequisiteService _prerequisiteService;
		private readonly IDataProvider _dataProvider;

		public ValidationProcessor(IModuleProcessor moduleProcessor, IOEProcessor oeProcessor, IValidationService validationService, IScenarioOEProvider scenarioOEProvider, ICapabilityService capabilityService, IPrerequisiteService prerequisiteService, IDataProvider dataProvider)
		{
			_moduleProcessor = moduleProcessor;
			_validationService = validationService;
			_scenarioOEProvider = scenarioOEProvider;
			_oeProcessor = oeProcessor;
			_capabilityService = capabilityService;
			_prerequisiteService = prerequisiteService;
			_dataProvider = dataProvider;
		}

		public InsertResult Create(NewRegistrationContainer foo)
		{
			//Create the "Module"
			var moduleResult = _moduleProcessor.Create(foo.Module);

			//Create the validation
			var validationCreateResult = _validationService.Create(ValidationSource.LCAVP, moduleResult.ID);

			foreach (var scenario in foo.Scenarios)
			{
				//Create the OE
				long oeID = _oeProcessor.Create(scenario.OE).ID;

				//Create the scenario algorithms, capabilities, prereqs
				foreach (var algorithmThingy in scenario.Algorithms)
				{
					//Create the validation OE algorithm record
					var validationOEAlgorithmCreateResult = _validationService.AddValidationOEAlgorithm(validationCreateResult.ID, oeID, algorithmThingy.Algorithm.AlgorithmID, -1);    //TODO - this -1 is here because the method was expecting ACVP, but there's no vector set here...

					//Create the capabilities under it
					CreateCapabilities(validationOEAlgorithmCreateResult.ID, algorithmThingy.Algorithm);

					//Create the prereqs under it
					CreatePrerequisites(validationCreateResult.ID, validationOEAlgorithmCreateResult.ID, algorithmThingy.Prerequisites);
				}
			}

			return validationCreateResult;
		}

		public void Update(UpdateRegistrationContainer foo)
		{
			//Add scenarios to the existing validation
			foreach (var scenario in foo.Scenarios)
			{
				//Try to find the existing OE, or create a new one
				(long OEID, bool IsNew) = GetOEID(scenario.OE, foo.ValidationID);

				//Get all the existing ValidationOEAlgorithms for this OE
				List<(long ValidationOEAlgorithmID, long AlgorithmID)> existingValidationOEAlgorithms = IsNew ? new List<(long ValidationOEAlgorithmID, long AlgorithmID)>() : _validationService.GetValidationOEAlgorithms(foo.ValidationID, OEID);

				//Create the scenario algorithms, capabilities, prereqs
				foreach (var algorithmThingy in scenario.Algorithms)
				{
					//Inactivate all valdiation OE algorithms for this algo - deleting capabilities and prereqs, and inactivating the validation OE algorithm itself
					foreach (var existingValidationOEAlgorithm in existingValidationOEAlgorithms.Where(x => x.AlgorithmID == algorithmThingy.Algorithm.AlgorithmID))
					{
						_validationService.InactivateValidationOEAlgorithm(existingValidationOEAlgorithm.ValidationOEAlgorithmID);
					}

					//Create the validation OE algorithm record
					var validationOEAlgorithmCreateResult = _validationService.AddValidationOEAlgorithm(foo.ValidationID, OEID, algorithmThingy.Algorithm.AlgorithmID, -1);    //TODO - this -1 is here because the method was expecting ACVP, but there's no vector set here...

					//Create the capabilities under it
					CreateCapabilities(validationOEAlgorithmCreateResult.ID, algorithmThingy.Algorithm);

					//Create the prereqs under it
					CreatePrerequisites(foo.ValidationID, validationOEAlgorithmCreateResult.ID, algorithmThingy.Prerequisites);
				}
			}
		}

		private void CreateCapabilities(long validationOEAlgorithmID, IAlgorithm algorithm)
		{
			//Convert it to a persistence algorithm
			IPersistedAlgorithm persistenceAlgorithm = algorithm.ToPersistedAlgorithm();

			//Persist it - the entire algorithm object is just a class as far as the persistence mechanism is concerned, just with some non-property properties on it
			_capabilityService.CreateClassCapabilities(algorithm.AlgorithmID, validationOEAlgorithmID, null, 0, null, persistenceAlgorithm);
		}

		private void CreatePrerequisites(long validationID, long validationOEAlgorithmID, List<Prerequisite> prereqs)
		{
			//Because of a weird old thing in how prereqs were being serialized, which I don't want to mess with, prereqs may be null instead of an empty list. So need to catch that
			foreach (Prerequisite prereq in prereqs ?? new List<Prerequisite>())
			{
				_prerequisiteService.Create(validationOEAlgorithmID, (long)(prereq.ValidationRecordID ?? validationID), prereq.Algorithm);  //self referential prereqs will have a null ValidationRecordID, so use the validation ID instead. Since the ValidationRecordID is an int? some casting has to be done, and this is the only syntax that will handle all cases
			}
		}

		private (long OEID, bool IsNew) GetOEID(OperationalEnvironment oe, long validationID)
		{
			long oeID;
			bool isNew = false;

			//Try to find an existing OE
			List<long> oeIDs = _dataProvider.GetOEIDsForValidation(validationID, oe.Name);

			if (oeIDs.Count == 0)
			{
				//Create a new OE
				oeID = _oeProcessor.Create(oe).ID;
				isNew = true;
			}
			else
			{
				// There's a decent chance that there's more than 1 OE with the same name, since we previously created a new OE each time. There's no good way to handle this. Just grab one?
				// The best thing would be to return a list, then in the caller "delete" all the existing ones
				// But hopefully this whole update process is only being used to add OEs...
				oeID = oeIDs[0];
			}

			return (oeID, isNew);
		}
	}
}
