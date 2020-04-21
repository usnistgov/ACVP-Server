using System;
using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.ACVPCore.Models;
using NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters;
using NIST.CVP.Libraries.Internal.ACVPCore.Results;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using Microsoft.AspNetCore.Mvc;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.Results;

namespace Web.Admin.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrganizationsController : ControllerBase
	{
		private readonly IOrganizationService _organizationService;

		public OrganizationsController(IOrganizationService organizationService)
		{
			_organizationService = organizationService;
		}

		[HttpDelete("{organizationId}")]
		public DeleteResult Delete(long organizationId)
		{
			return _organizationService.Delete(organizationId);
		}

		[HttpGet("{organizationId}")]
		public Organization Get(long organizationId)
		{
			return _organizationService.Get(organizationId);
		}

		[HttpPatch("{organizationId}")]
		public OrganizationResult Update(long organizationId, Organization organization)
		{

			OrganizationUpdateParameters parameters = new OrganizationUpdateParameters();

			parameters.ID = organizationId;

			if (organization.Name != null)
			{
				parameters.Name = organization.Name;
				parameters.NameUpdated = true;
			}
			if (organization.Url != null)
			{
				parameters.Website = organization.Url;
				parameters.WebsiteUpdated = true;
			}
			if (organization.VoiceNumber != null)
			{
				parameters.VoiceNumber = organization.VoiceNumber;
				parameters.VoiceNumberUpdated = true;
			}
			if (organization.FaxNumber != null)
			{
				parameters.FaxNumber = organization.FaxNumber;
				parameters.FaxNumberUpdated = true;
			}
			return _organizationService.Update(parameters);
		}

		[HttpDelete("{organizationId}/addresses/{indexOfAddressToRemove}")]
		public Result DeleteAddress(long organizationId, int indexOfAddressToRemove)
		{
			// Get the current set of Addresses
			Organization selectedOrganization = _organizationService.Get(organizationId);

			// Create the update params and copy in the current addresses
			OrganizationUpdateParameters parameters = new OrganizationUpdateParameters();

			// Instantiate the necessary child objects
			parameters.ID = organizationId;
			parameters.Addresses = new List<object>();
			parameters.AddressesUpdated = true;

			// Convert to a more generic form to be consumed by the service, while removing the one being deleted
			List<object> genericList = new List<object>();
			for (int i = 0; i < selectedOrganization.Addresses.Count; i++)
			{
				Console.WriteLine("I = " + i);
				if (i != indexOfAddressToRemove)
				{
					Console.WriteLine("IndexToRemove = " + indexOfAddressToRemove);
					//genericList.Add(selectedOrganization.Addresses[i]);
					genericList.Add(new AddressUpdateParameters
					{
						ID = selectedOrganization.Addresses[i].ID
					});
				}
			}

			parameters.Addresses = genericList;

			// Issue the update
			return _organizationService.Update(parameters);
		}

		[HttpPost]
		public ActionResult<PagedEnumerable<OrganizationLite>> GetDependencies(OrganizationListParameters param)
		{
			if (param == null)
				return new BadRequestResult();

			return _organizationService.Get(param);
		}

	}
}