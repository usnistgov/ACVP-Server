using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ACVPCore;
using ACVPCore.Algorithms;
using ACVPCore.Algorithms.External;
using ACVPCore.Algorithms.Persisted;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Models;
using ACVPWorkflow.Services;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class CertifyTestSessionProcessor : IWorkflowItemProcessor
	{
		private readonly IValidationService _validationService;
		private readonly ITestSessionService _testSessionService;
		private readonly IVectorSetService _vectorSetService;
		private readonly IWorkflowService _workflowService;

		public CertifyTestSessionProcessor(IValidationService validationService, ITestSessionService testSessionService, IVectorSetService vectorSetService, IWorkflowService workflowService)
		{
			_validationService = validationService;
			_testSessionService = testSessionService;
			_vectorSetService = vectorSetService;
			_workflowService = workflowService;
		}

		public void Approve(WorkflowItem workflowItem)
		{
			CertifyTestSessionParameters parameters = JsonSerializer.Deserialize<CertifyTestSessionPayload>(workflowItem.JSON).ToCertifyTestSessionParameters();

			//This is the ugly stuff...

			//For each vector set that is passed (ignore cancelled ones, and anything else, though they shouldn't get to that point if there's anything else), turn the registration into proto capabilities
			var vectorSets = _vectorSetService.GetVectorSetsForTestSession(parameters.TestSessionID);

//TODO - With the newest approach we decided on, need to change this... Going to do complete replacement of an algorithm with the new stuff.
//But this is complicated by the fact that several algorithms may need multiple registrations
//So really need to group these by algorithm, at least if it is an update. Then "delete" all the existing data, and replace with all the new registrations

			foreach (var vectorSet in vectorSets.Where(x => x.Status == VectorSetStatus.Passed))
			{
				//Get the registration from the crazy place they are found...
				long algorithmID = vectorSet.AlgorithmID;
				string registrationJSON = _vectorSetService.GetCapabilities(vectorSet.ID);

				//Deserialize the JSON
				IExternalAlgorithm externalAlgorithm = ExternalAlgorithmFactory.Deserialize(registrationJSON);

				//Convert the external algorithm to a persistence algorithm

				//	Convert the registration to capabilities
				//	Add the prereqs for this algo
				//		If not self referential do a lookup - think this is only LCAVP relevant - based on source and ID

			}



			//Look for an existing validation for this product - logic here needs to change about what to update - ACVP should only update A validations. There's a chance that there are multiple if we've done goofy things to the data, so just take the latest - not going to try to figure out if it matches better with one of multiple validations. Should be an extremely rare case.
			long validationID = _validationService.GetLatestACVPValidationForImplementation(parameters.ImplementationID);

			if (validationID == 0)
			{
				//Create a new validation, all new stuff
				InsertResult createValidationResult = _validationService.Create(ValidationSource.ACVP, parameters.ImplementationID, parameters.OEID);
			}
			else
			{
				//Update the existing validation - use the "merge" logic to determine if we update an existing scenario or create a new one
				long scenarioID = _validationService.FindMatchingScenario(validationID, null);      //TODO - this is a collection of scenario algorithms

				Result updateResult;

				//How we update it depends on whether or not a match was found
				if (scenarioID == -1)
				{
					//Don't have a an existing matching scenario, so need to create one and everything underneath it
					updateResult = _validationService.CreateScenario(validationID, parameters.OEID);
				}
				else
				{
					//This is simply an OE add to an existing scenario
					updateResult = _validationService.AddOEToScenario(scenarioID, -1);	//TODO get the right OE ID
				}
			}

			//Update test session to published
			_testSessionService.UpdateStatus(parameters.TestSessionID, TestSessionStatus.Published);

			//Update the workflow item
			//if (validationResult.IsSuccess)
			//{
			//	_workflowService.MarkApproved(workflowItem.WorkflowItemID, validationResult.ID);
			//}
		}

		public void Reject(WorkflowItem workflowItem) { }


		//This probably belongs in the validation service, but doing here for clarity during development
		public void CreateACVPValidation(long implementationID, long oeID)
		{
			//Get what the validation number to assign to it
			long validationNumber = _validationService.GetValidationNumber(ValidationSource.ACVP);

			if (validationNumber == -1) return;	//TODO better error handling

			//Create the validation record
			long validationID = _validationService.Create()
		}

		private IPersistedAlgorithm ConvertToPersistedAlgorithm(IExternalAlgorithm externalAlgorithm)
		{

		}
	}
}
