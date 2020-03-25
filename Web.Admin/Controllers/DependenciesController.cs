using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using Microsoft.AspNetCore.Mvc;
using NIST.CVP.Enumerables;
using NIST.CVP.Results;

namespace Web.Admin.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DependenciesController : BaseAuthorizedController
	{
		private readonly IDependencyService _dependencyService;

		public DependenciesController(IDependencyService dependencyService)
		{
			_dependencyService = dependencyService;
		}

		[HttpPost("{dependencyId}/attributes")]
		public InsertResult AddAttribute(long dependencyId, [FromBody] DependencyAttribute attribute)
		{
			return _dependencyService.InsertAttribute(dependencyId, attribute.Name, attribute.Value);
		}

		[HttpDelete("{dependencyId}/attributes/{attributeId}")]
		public Result DeleteAttribute(long attributeId)
		{
			return _dependencyService.DeleteAttribute(attributeId);
		}

		/// <summary>
		/// The function for processing PATCH-es to a dependency (to update its main values)
		/// - Submit nulls for Name, type, or Description if no change requested
		/// </summary>
		[HttpPatch("{dependencyId}")]
		public Result UpdateDependency(Dependency dependency)
		{
			DependencyUpdateParameters updateParams = new DependencyUpdateParameters();

			if (dependency.Name != null)
			{
				updateParams.Name = dependency.Name;
				updateParams.NameUpdated = true;
			}
			if (dependency.Type != null)
			{
				updateParams.Type = dependency.Type;
				updateParams.TypeUpdated = true;
			}
			if (dependency.Description != null)
			{
				updateParams.Description = dependency.Description;
				updateParams.DescriptionUpdated = true;
			}
			updateParams.ID = dependency.ID;

			return _dependencyService.Update(updateParams);
		}

		[HttpGet("{dependencyId}")]
		public Dependency GetDependency(long dependencyId)
		{
			return _dependencyService.Get(dependencyId);
		}

        [HttpPost("create")]
        public DependencyResult CreateDependency([FromBody] DependencyCreateParameters parameters)
        {
            return _dependencyService.Create(parameters);
        }

        [HttpPost]
        public ActionResult<PagedEnumerable<Dependency>> GetDependencies(DependencyListParameters param)
        {
            if (param == null)
                return new BadRequestResult();
            
            return _dependencyService.Get(param);
        }
    }
}