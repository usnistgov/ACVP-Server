using NIST.CVP.Libraries.Internal.ACVPCore;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using Microsoft.AspNetCore.Mvc;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.Results;

namespace Web.Admin.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class ImplementationsController : ControllerBase
    {
        private readonly IImplementationService _implementationService;

        public ImplementationsController(IImplementationService implementationService)
        {
            _implementationService = implementationService;
        }

		[HttpGet("{implementationId}")]
		public Implementation Get(long implementationId)
		{
			return _implementationService.Get(implementationId);
		}

        [HttpPost]
        public ActionResult<PagedEnumerable<Implementation>> ListImplementations(ImplementationListParameters param)
        {
            if (param == null)
                return new BadRequestResult();
            
            return _implementationService.ListImplementations(param);
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
			if (implementation.Type != ImplementationType.Unknown)
			{
				parameters.Type = ImplementationTypeExtensions.FromString(implementation.Type.ToString());
				parameters.TypeUpdated = true;
			}
			parameters.ID = implementation.ID;
			return _implementationService.Update(parameters);
		}
	}
}