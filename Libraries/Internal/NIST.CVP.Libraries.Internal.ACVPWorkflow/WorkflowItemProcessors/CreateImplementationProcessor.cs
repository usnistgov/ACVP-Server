using System.Linq;
using NIST.CVP.Libraries.Internal.ACVPCore;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Results;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Exceptions;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemProcessors
{
	public class CreateImplementationProcessor : BaseWorkflowItemProcessor, IWorkflowItemProcessor
	{
		private readonly IImplementationService _implementationService;
		private readonly IAddressService _addressService;
		private readonly IWorkflowItemPayloadValidatorFactory _workflowItemPayloadValidatorFactory;

		public CreateImplementationProcessor(IImplementationService implementationService, IAddressService addressService, IWorkflowItemPayloadValidatorFactory workflowItemPayloadValidatorFactory)
		{
			_implementationService = implementationService;
			_addressService = addressService;
			_workflowItemPayloadValidatorFactory = workflowItemPayloadValidatorFactory;
		}

		public bool Validate(WorkflowItem workflowItem)
		{
			return IsPendingApproval(workflowItem) && _workflowItemPayloadValidatorFactory.GetWorkflowItemPayloadValidator(APIAction.CreateImplementation).Validate((ImplementationCreatePayload)workflowItem.Payload);
		}

		public long Approve(WorkflowItem workflowItem)
		{
			//Validate this workflow item
			Validate(workflowItem);

			ImplementationCreatePayload payload = (ImplementationCreatePayload)workflowItem.Payload;

			//Do most of the conversion to a parameters object...
			ImplementationCreateParameters parameters = new ImplementationCreateParameters
			{
				Name = payload.Name,
				Description = payload.Description,
				Type = ParseImplementationType(payload.Type),
				Version = payload.Version,
				Website = payload.Website,
				//OrganizationID = ParseIDFromURL(VendorURL),			//return to this in the future
				OrganizationID = payload.VendorObjectThatNeedsToGoAway.ID,
				AddressID = GetAddressID(payload.AddressURL, payload.VendorObjectThatNeedsToGoAway.ID),
				//ContactIDs = ContactURLs?.Select(x => ParseIDFromURL(x)).ToList(),		//return to this in the future
				ContactIDs = payload.ContactsObjectThatNeedsToGoAway?.OrderBy(x => x.OrderIndex).Select(x => x.Person.ID).ToList(),
				IsITAR = false      //TODO - Do something for ITARs. For now, assuming nothing is ITAR
			};

			//Create it
			ImplementationResult implementationCreateResult = _implementationService.Create(parameters);

			if (!implementationCreateResult.IsSuccess)
			{
				throw new ResourceProcessorException($"Failed approval on {nameof(workflowItem.APIAction)} {workflowItem.APIAction}");
			}

			return implementationCreateResult.ID;
		}

		public void Reject(WorkflowItem workflowItem) { }

		private ImplementationType ParseImplementationType(string type) => type.ToLower() switch
		{
			"software" => ImplementationType.Software,
			"hardware" => ImplementationType.Hardware,
			"firmware" => ImplementationType.Firmware,
			_ => ImplementationType.Unknown
		};

		private long GetAddressID(string addressURL, long organizationID)
		{
			//Parse the address ID, but it might be null
			long? addressID = BasePayload.ParseNullableIDFromURL(addressURL);

			if (addressID != null)
			{
				return (long)addressID;
			}

			//If it was null, need to get an address off the vendor
			addressID = _addressService.GetAllForOrganization(organizationID).FirstOrDefault()?.ID;

			if (addressID == null)
			{
				throw new ResourceDoesNotExistException("Implementation create payload does not include address, and the referenced vendor has no addresses");
			}

			//If we made it here we know we have a good ID, so use it
			return (long)addressID;
		}
	}
}
