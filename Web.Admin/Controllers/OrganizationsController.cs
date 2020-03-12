using System;
using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using Microsoft.AspNetCore.Mvc;

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

		//[HttpGet]
		//public WrappedEnumerable<Implementation> ListImplementations(long pageSize, long pageNumber)
		//{
		//    // Set some defaults in case no values are provided
		//    if (pageSize == 0) { pageSize = 10; }
		//    if (pageNumber == 0) { pageNumber = 1; }
		//    return _implementationService.ListImplementations(pageSize, pageNumber).WrapEnumerable();
		//}

		//[HttpPatch("{implementationId}")]
		//public Result UpdateImplementation(Implementation implementation)
		//{
		//    ImplementationUpdateParameters parameters = new ImplementationUpdateParameters();

		//    if (implementation.Description != null)
		//    {
		//        parameters.Description = implementation.Description;
		//        parameters.DescriptionUpdated = true;
		//    }
		//    if (implementation.Name != null)
		//    {
		//        parameters.Name = implementation.Name;
		//        parameters.NameUpdated = true;
		//    }
		//    if (implementation.URL != null)
		//    {
		//        parameters.Website = implementation.URL;
		//        parameters.WebsiteUpdated = true;
		//    }
		//    if (implementation.Version != null)
		//    {
		//        parameters.Version = implementation.Version;
		//        parameters.VersionUpdated = true;
		//    }
		//    if (implementation.Vendor != null)
		//    {
		//        parameters.OrganizationID = implementation.Vendor.ID;
		//        parameters.OrganizationIDUpdated = true;
		//    }
		//    if (implementation.Type != null)
		//    {
		//        parameters.Type = ImplementationTypeExtensions.FromString(implementation.Type.ToString());
		//        parameters.TypeUpdated = true;
		//    }
		//    parameters.ID = implementation.ID;
		//    return _implementationService.Update(parameters);
		//}
	}
}