using NIST.CVP.Libraries.Internal.ACVPCore;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Exceptions;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemPayloadValidators
{
	public class CertifyTestSessionPayloadValidator : IWorkflowItemPayloadValidator
	{
		private readonly IImplementationService _implementationService;
		private readonly IOEService _oeService;
		private readonly ITestSessionService _testSessionService;
		private readonly IWorkflowItemPayloadValidatorFactory _workflowItemPayloadValidatorFactory;

		public CertifyTestSessionPayloadValidator(IImplementationService implementationService, IOEService oeService, ITestSessionService testSessionService, IWorkflowItemPayloadValidatorFactory workflowItemPayloadValidatorFactory)
		{
			_implementationService = implementationService;
			_oeService = oeService;
			_testSessionService = testSessionService;
			_workflowItemPayloadValidatorFactory = workflowItemPayloadValidatorFactory;
		}

		public bool Validate(IWorkflowItemPayload workflowItemPayload)
		{
			CertifyTestSessionPayload certifyTestSessionPayload = (CertifyTestSessionPayload)workflowItemPayload;
			CertifyTestSessionParameters parameters = certifyTestSessionPayload.ToCertifyTestSessionParameters();

			//Verify the test session exists
			if (!_testSessionService.TestSessionExists(parameters.TestSessionID))
			{
				throw new ResourceDoesNotExistException($"Test Session {parameters.TestSessionID} does not exist");
			}

			//Verify that the test session is in the appropriate status
			if (_testSessionService.GetStatus(certifyTestSessionPayload.TestSessionID) != TestSessionStatus.SubmittedForApproval)
			{
				throw new BusinessRuleException($"Test session {parameters.TestSessionID} not in Submitted For Approval status");
			}

			//Verify the implementation - could be inline or a reference
			if (certifyTestSessionPayload.ImplementationToCreate == null)
			{
				//Verify the referenced implementation exists
				long implementationID = BasePayload.ParseIDFromURL(certifyTestSessionPayload.ImplementationURL);

				if (!_implementationService.ImplementationExists(implementationID))
				{
					throw new ResourceDoesNotExistException($"Implementation {implementationID} does not exist");
				}
			}
			else
			{
				//Verify that the implementation create is good
				_workflowItemPayloadValidatorFactory.GetWorkflowItemPayloadValidator(APIAction.CreateImplementation).Validate(certifyTestSessionPayload.ImplementationToCreate);
			}


			if (certifyTestSessionPayload.OEToCreate == null)
			{
				long oeID = BasePayload.ParseIDFromURL(certifyTestSessionPayload.OEURL);

				if (!_oeService.OEExists(oeID))
				{
					throw new ResourceDoesNotExistException($"OE {oeID} does not exist");
				}
			}
			else
			{
				//Verify that the OE create is good
				_workflowItemPayloadValidatorFactory.GetWorkflowItemPayloadValidator(APIAction.CreateOE).Validate(certifyTestSessionPayload.OEToCreate);
			}
			

			return true;
		}
	}
}
