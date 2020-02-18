using ACVPCore;
using ACVPCore.ExtensionMethods;
using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Web.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IImplementationService _implementationService;
        private readonly IAddressService _addressService;

        public ProductsController(
           IImplementationService implementationService,
           IAddressService addressService)
        {
            _implementationService = implementationService;
        }

        [HttpGet("{implementationId}")]
        public Implementation Get(long implementationId)
        {
            return _implementationService.Get(implementationId);
        }

        [HttpGet]
        public WrappedEnumerable<Implementation> ListImplementations(long pageSize, long pageNumber)
        {
            // Set some defaults in case no values are provided
            if (pageSize == 0) { pageSize = 10; }
            if (pageNumber == 0) { pageNumber = 1; }
            return _implementationService.ListImplementations(pageSize, pageNumber).WrapEnumerable();
        }

        [HttpPatch("{implementationId}")]
        public Result UpdateImplementation(Implementation implementation)
        {
            ImplementationUpdateParameters parameters = new ImplementationUpdateParameters();

            if (implementation.Description != null)
            {
                parameters.Description = implementation.Description;
                parameters.DescriptionUpdated = true;
            }
            if (implementation.Name != null)
            {
                parameters.Name = implementation.Name;
                parameters.NameUpdated = true;
            }
            if (implementation.URL != null)
            {
                parameters.Website = implementation.URL;
                parameters.WebsiteUpdated = true;
            }
            if (implementation.Version != null)
            {
                parameters.Version = implementation.Version;
                parameters.VersionUpdated = true;
            }
            if (implementation.Vendor != null)
            {
                parameters.OrganizationID = implementation.Vendor.ID;
                parameters.OrganizationIDUpdated = true;
            }
            if (implementation.Type != null)
            {
                parameters.Type = ImplementationTypeExtensions.FromString(implementation.Type.ToString());
                parameters.TypeUpdated = true;
            }
            parameters.ID = implementation.ID;
            return _implementationService.Update(parameters);
        }
    }
}