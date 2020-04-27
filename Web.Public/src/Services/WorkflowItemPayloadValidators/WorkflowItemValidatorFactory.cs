using System;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;

namespace Web.Public.Services.WorkflowItemPayloadValidators
{
	public class WorkflowItemValidatorFactory : IWorkflowItemValidatorFactory
	{
		private readonly IDependencyService _dependencyService;
		private readonly IAddressService _addressService;
		private readonly IOrganizationService _organizationService;
		private readonly IPersonService _personService;
		private readonly ITestSessionService _testSessionService;
		private readonly IImplementationService _implementationService;
		private readonly IParameterValidatorService _parameterValidatorService;
		private readonly IOEService _oeService;

		public WorkflowItemValidatorFactory(IDependencyService dependencyService, IAddressService addressService, IOrganizationService organizationService, IPersonService personService, ITestSessionService testSessionService, IImplementationService implementationService, IParameterValidatorService parameterValidatorService, IOEService oeService)
		{
			_dependencyService = dependencyService;
			_addressService = addressService;
			_organizationService = organizationService;
			_personService = personService;
			_testSessionService = testSessionService;
			_implementationService = implementationService;
			_parameterValidatorService = parameterValidatorService;
			_oeService = oeService;
			_parameterValidatorService = parameterValidatorService;
		}
		
		public IWorkflowItemValidator GetWorkflowItemPayloadValidator(APIAction action)
		{
			return action switch
			{
				APIAction.CreateDependency => new DependencyCreatePayloadValidator(),
				APIAction.CreateImplementation => new ImplementationCreatePayloadValidator(_addressService, _organizationService, _personService),
				APIAction.CreateOE => new OperatingEnvironmentCreatePayloadValidator(this, _dependencyService),
				APIAction.CreatePerson => new PersonCreatePayloadValidator(),
				APIAction.CreateVendor => new VendorCreatePayloadValidator(),
				APIAction.DeleteDependency => new DependencyDeletePayloadValidator(_dependencyService),
				APIAction.DeleteImplementation => new ImplementationDeletePayloadValidator(_implementationService),
				APIAction.DeleteOE => new OperatingEnvironmentDeletePayloadValidator(_oeService),
				APIAction.DeletePerson => new PersonDeletePayloadValidator(),
				APIAction.DeleteVendor => new VendorDeletePayloadValidator(),
				APIAction.UpdateDependency => new DependencyUpdatePayloadValidator(_dependencyService),
				APIAction.UpdateImplementation => new ImplementationUpdatePayloadValidator(_implementationService, _addressService, _organizationService, _personService),
				APIAction.UpdateOE => new OperatingEnvironmentUpdatePayloadValidator(this, _dependencyService),
				APIAction.UpdatePerson => new PersonUpdatePayloadValidator(),
				APIAction.UpdateVendor => new VendorUpdatePayloadValidator(),
				APIAction.RegisterTestSession => new RegisterTestSessionPayloadValidator(_parameterValidatorService),
				APIAction.CertifyTestSession => new CertifyTestSessionPayloadValidator(_testSessionService),
				APIAction.CancelTestSession => new CancelTestSessionPayloadValidator(_testSessionService),
				APIAction.CancelVectorSet => new CancelVectorSetPayloadValidator(_testSessionService),
				APIAction.ResubmitVectorSetResults => new ResubmitVectorSetResultsPayloadValidator(),
				APIAction.SubmitVectorSetResults => new SubmitVectorSetResultsPayloadValidator(),
				_ => throw new ArgumentException($"Invalid {nameof(action)}: {action}")
			};
		}
	}
}