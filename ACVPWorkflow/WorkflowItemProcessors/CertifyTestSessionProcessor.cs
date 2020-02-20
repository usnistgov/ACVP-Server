using System.Collections.Generic;
using System.Linq;
using ACVPCore;
using ACVPCore.Algorithms;
using ACVPCore.Algorithms.External;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Models;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class CertifyTestSessionProcessor : BaseWorkflowItemProcessor, IWorkflowItemProcessor
	{
		private readonly IValidationService _validationService;
		private readonly ITestSessionService _testSessionService;
		private readonly IVectorSetService _vectorSetService;
		private readonly IDependencyService _dependencyService;
		private readonly IOEService _oeService;
		private readonly IImplementationService _implementationService;
		private readonly IWorkflowItemPayloadValidatorFactory _workflowItemPayloadValidatorFactory;

		public CertifyTestSessionProcessor(
			IValidationService validationService, 
			ITestSessionService testSessionService, 
			IVectorSetService vectorSetService, 
			IDependencyService dependencyService, 
			IOEService oeService, 
			IImplementationService implementationService,
			IWorkflowItemPayloadValidatorFactory workflowItemPayloadValidatorFactory)
		{
			_validationService = validationService;
			_testSessionService = testSessionService;
			_vectorSetService = vectorSetService;
			_dependencyService = dependencyService;
			_oeService = oeService;
			_implementationService = implementationService;
			_workflowItemPayloadValidatorFactory = workflowItemPayloadValidatorFactory;
		}

		public bool Validate(WorkflowItem workflowItem)
		{
			return IsPendingApproval(workflowItem) && _workflowItemPayloadValidatorFactory.GetWorkflowItemPayloadValidator(APIAction.CertifyTestSession).Validate((CertifyTestSessionPayload)workflowItem.Payload);
		}

		public long Approve(WorkflowItem workflowItem)
		{
			//Validate this workflow item
			Validate(workflowItem);

			CertifyTestSessionPayload certifyTestSessionPayload = (CertifyTestSessionPayload)workflowItem.Payload;
			CertifyTestSessionParameters parameters = certifyTestSessionPayload.ToCertifyTestSessionParameters();

			//May need to create an implementation since once can be declared inline...
			bool isNewImplementation = false;
			if (certifyTestSessionPayload.ImplementationToCreate != null)
			{
				isNewImplementation = true;

				//Create an implementation and set the ID in the parameters to the new ID
				parameters.ImplementationID = CreateInlineImplementation(certifyTestSessionPayload.ImplementationToCreate).ID;
			}

			//May need to create an OE/Dependencies since they can be declared inline...
			bool isNewOE = false;
			if (certifyTestSessionPayload.OEToCreate != null)
			{
				isNewOE = true;

				//Create an OE and set the ID in the parameters to the new ID
				parameters.OEID = CreateInlineOE(certifyTestSessionPayload.OEToCreate).ID;
			}

			//Get or create the validation ID
			(long validationID, bool isNewValidation) = GetValidationID(parameters.ImplementationID, isNewImplementation);

			//Get or create the scenario ID
			(long scenarioID, bool isNewScenario) = GetScenarioID(validationID, parameters.OEID, isNewValidation, isNewOE);

			//Get all the existing scenario algorithms, as we'll be replacing some or all of these - know that it is an empty collection if a new scenario
			List<(long ScenarioAlgorithmID, long AlgorithmID)> existingScenarioAlgorithms = isNewScenario ? new List<(long ScenarioAlgorithmID, long AlgorithmID)>() : _validationService.GetScenarioAlgorithms(scenarioID);

			//Get the passed vector sets (ignore cancelled ones, and anything else, though they shouldn't get to that point if there's anything else)
			var vectorSets = _vectorSetService.GetVectorSetsForTestSession(parameters.TestSessionID).Where(x => x.Status == VectorSetStatus.Passed);

			//Get the list of distinct algorithms in those vector sets so we can work one algo at a time
			var algorithms = vectorSets.Select(x => x.AlgorithmID).Distinct();

			foreach (long algorithmID in algorithms)
			{
				//Do whatever massaging of the prereqs because the same thing will be inserted for each instance of the algo
				//Look up the algorithm ID based on the name and mode on the prerequisite - there is no revision... Because of this, name/mode may return multiple algos
				//Get the ones that match this algorithm

				//If this is an existing scenario there may be existing data for this algo that needs to be deleted. Rather than checking first if the algo exists, just delete, may or may not delete anything
				if (!isNewScenario)
				{
					//Delete all scenario algorithms for this algo - meaning capabilities, prereqs, and the scenario algorithm itself
					foreach (long existingScenarioAlgorithmID in existingScenarioAlgorithms.Where(x => x.AlgorithmID == algorithmID).Select(x => x.ScenarioAlgorithmID))
					{
						_validationService.DeleteScenarioAlgorithm(existingScenarioAlgorithmID);
					}
				}

				//Add scenario algorithm/capabilities/prereqs for each instance of this algorithm
				foreach (var vectorSet in vectorSets.Where(x => x.AlgorithmID == algorithmID))
				{
					//Create the scenario algorithm
					InsertResult scenarioAlgorithmInsertResult = _validationService.AddScenarioAlgorithm(scenarioID, algorithmID);
					long scenarioAlgorithmID = scenarioAlgorithmInsertResult.ID;

					//Get the registration JSON
					string registrationJSON = _vectorSetService.GetVectorFileJson(vectorSet.ID, VectorSetJsonFileTypes.Capabilities);

					//Deserialize that JSON
					IExternalAlgorithm externalAlgorithm = ExternalAlgorithmFactory.Deserialize(registrationJSON);

					//Add the capabilities based on that algorithm object
					_validationService.CreateCapabilities(algorithmID, scenarioAlgorithmID, externalAlgorithm);

					//Add the prereqs
					//TODO - this
				}
			}

			//Update test session to published
			_testSessionService.UpdateStatus(parameters.TestSessionID, TestSessionStatus.Published);

			//Log which validation the test session is a part of
			_validationService.LogValidationTestSession(validationID, parameters.TestSessionID);

			return validationID;
		}

		public void Reject(WorkflowItem workflowItem) { }


		private InsertResult CreateInlineImplementation(ImplementationCreatePayload implementationCreatePayload)
		{
			//Convert the payload to create parameters
			ImplementationCreateParameters implementationCreateParameters = implementationCreatePayload.ToImplementationCreateParameters();

			//Create the implementation
			return _implementationService.Create(implementationCreateParameters);
		}

		private InsertResult CreateInlineOE(OECreatePayload oeCreatePayload)
		{
			//Convert the payload to create parameters
			OECreateParameters oeCreateParameters = oeCreatePayload.ToOECreateParameters();

			//If there were any new Dependencies in the OE, instead of just URLs, create those and add them to the collection of dependency IDs in the OE create parameters
			foreach (DependencyCreatePayload dependencyCreatePayload in oeCreatePayload.DependenciesToCreate)
			{
				//Convert from a payload to parameters
				DependencyCreateParameters dependencyCreateParameters = dependencyCreatePayload.ToDependencyCreateParameters();

				//Create it
				DependencyResult dependencyCreateResult = _dependencyService.Create(dependencyCreateParameters);

				//Add it to the dependency list
				if (dependencyCreateResult.IsSuccess)
				{
					oeCreateParameters.DependencyIDs.Add(dependencyCreateResult.ID);
				}
			}

			//Create the OE
			return _oeService.Create(oeCreateParameters);
		}

		private (long ValidationID, bool IsNewValidation) GetValidationID(long implementationID, bool isNewImplementation)
		{
			//Find/create the validation for this implementation - ACVP should only update A validations. There's a chance that there are multiple if we've done goofy things to the data, so just take the latest - not going to try to figure out if it matches better with one of multiple validations. Should be an extremely rare case.
			bool isNewValidation = isNewImplementation;     //Know that a new implementation means a new validation, so can skip a check
			long validationID = 0;

			if (!isNewImplementation)
			{
				//Look for an ACVP validation since the implementation exists
				validationID = _validationService.GetLatestACVPValidationForImplementation(implementationID);

				isNewValidation = validationID == 0;
			}

			//Create the validation if didn't find one
			if (isNewValidation)
			{
				InsertResult createValidationResult = _validationService.Create(ValidationSource.ACVP, implementationID);
				validationID = createValidationResult.ID;
			}

			return (validationID, isNewValidation);
		}

		private (long ScenarioID, bool IsNewScenario) GetScenarioID(long validationID, long oeID, bool isNewValidation, bool isNewOE)
		{
			long scenarioID = 0;
			bool isNewScenario = isNewValidation || isNewOE;        //Know that it is definitely a new scenario if it is a new validation or a newly created OE

			//If we don't know that this is a new scenario then look for it
			if (!isNewScenario)
			{
				scenarioID = _validationService.GetScenarioIDForValidationOE(validationID, oeID);
				isNewScenario = scenarioID == 0;
			}

			//If no existing scenario found, create one
			if (isNewScenario)
			{
				InsertResult createScenarioResult = _validationService.CreateScenario(validationID, oeID);
				scenarioID = createScenarioResult.ID;
			}

			return (scenarioID, isNewScenario);
		}
	}
}
