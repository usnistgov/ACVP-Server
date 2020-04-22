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

		public ValidationProcessor(IModuleProcessor moduleProcessor, IOEProcessor oeProcessor, IValidationService validationService, IScenarioOEProvider scenarioOEProvider, ICapabilityService capabilityService, IPrerequisiteService prerequisiteService)
		{
			_moduleProcessor = moduleProcessor;
			_validationService = validationService;
			_scenarioOEProvider = scenarioOEProvider;
			_oeProcessor = oeProcessor;
			_capabilityService = capabilityService;
			_prerequisiteService = prerequisiteService;
		}

		public InsertResult Create(NewRegistrationContainer foo)
		{
			//Create the "Module"
			var moduleResult = _moduleProcessor.Create(foo.Module);

			//Create the validation
			var validationCreateResult = _validationService.Create(ValidationSource.LCAVP, moduleResult.ID);

			foreach (var scenario in foo.Scenarios)
			{
				//Create the scenario record, complete with all the OEs
				long scenarioID = CreateScenario(validationCreateResult.ID, scenario.OEs);

				//Create the scenario algorithms, capabilities, prereqs
				foreach (var algorithmThingy in scenario.Algorithms)
				{
					//Create the scenario algorithm record
					var scenarioAlgorithmResult = _validationService.AddScenarioAlgorithm(scenarioID, algorithmThingy.Algorithm.AlgorithmID);

					//Create the capabilities under it
					CreateCapabilities(scenarioAlgorithmResult.ID, algorithmThingy.Algorithm);

					//Create the prereqs under it
					CreatePrerequisites(validationCreateResult.ID, scenarioAlgorithmResult.ID, algorithmThingy.Prerequisites);
				}
			}

			return validationCreateResult;
		}

		public void Update(UpdateRegistrationContainer foo)
		{
			//Add scenarios to the existing validation
			foreach (var scenario in foo.Scenarios)
			{
				//Create the scenario record, complete with all the OEs
				long scenarioID = CreateScenario(foo.ValidationID, scenario.OEs);

				//Create the scenario algorithms, capabilities, prereqs
				foreach (var algorithmThingy in scenario.Algorithms)
				{
					//Create the scenario algorithm record
					var scenarioAlgorithmResult = _validationService.AddScenarioAlgorithm(scenarioID, algorithmThingy.Algorithm.AlgorithmID);

					//Create the capabilities under it
					CreateCapabilities(scenarioAlgorithmResult.ID, algorithmThingy.Algorithm);

					//Create the prereqs under it
					CreatePrerequisites(foo.ValidationID, scenarioAlgorithmResult.ID, algorithmThingy.Prerequisites);
				}
			}
		}


		private Result AddOEToScenario(long scenarioID, long oeID) => _scenarioOEProvider.Insert(scenarioID, oeID);

		private long CreateScenario(long validationID, List<OperationalEnvironment> oes)
		{
			List<long> oeIDs = new List<long>();

			//Create each of the OEs in the scenario
			foreach (var oe in oes)
			{
				var oeResult = _oeProcessor.Create(oe);
				oeIDs.Add(oeResult.ID);
			}

			//Create the scenario - this will add the first OE
			var scenarioCreateResult = _validationService.CreateScenario(validationID, oeIDs[0]);

			//Add any remaining OEs to the scenario
			oeIDs.Skip(1).ToList().ForEach(x => AddOEToScenario(scenarioCreateResult.ID, x));

			return scenarioCreateResult.ID;
		}

		private void CreateCapabilities(long scenarioAlgorithmID, IAlgorithm algorithm)
		{
			//Convert it to a persistence algorithm
			IPersistedAlgorithm persistenceAlgorithm = algorithm.ToPersistedAlgorithm();

			//Persist it - the entire algorithm object is just a class as far as the persistence mechanism is concerned, just with some non-property properties on it
			_capabilityService.CreateClassCapabilities(algorithm.AlgorithmID, scenarioAlgorithmID, null, null, 0, 0, null, persistenceAlgorithm);
		}

		private void CreatePrerequisites(long validationID, long scenarioAlgorithmID, List<Prerequisite> prereqs)
		{
			//Because of a weird old thing in how prereqs were being serialized, which I don't want to mess with, prereqs may be null instead of an empty list. So need to catch that
			foreach (Prerequisite prereq in prereqs ?? new List<Prerequisite>())
			{
				_prerequisiteService.Create(scenarioAlgorithmID, (long)(prereq.ValidationRecordID ?? validationID), prereq.Algorithm);	//self referential prereqs will have a null ValidationRecordID, so use the validation ID instead. Since the ValidationRecordID is an int? some casting has to be done, and this is the only syntax that will handle all cases
			}
		}
	}
}
