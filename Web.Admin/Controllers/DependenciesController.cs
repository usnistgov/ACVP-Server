using ACVPCore.ExtensionMethods;
using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DependenciesController : ControllerBase
    {
        private readonly IDependencyService _dependencyService;

        public DependenciesController(
           IDependencyService dependencyService)
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

            if (dependency.Name != null) {
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

        //[HttpGet]
        //public WrappedEnumerable<Dependency> GetDependencies()
        //{
        //    return _dependencyService.Get(1, 10).WrapEnumerable();
        //}

        [HttpGet]
        public WrappedEnumerable<Dependency> GetDependencies(long pageSize, long pageNumber)
        {

            // Set some defaults in case no values are provided
            if(pageSize == 0) { pageSize = 10; }
            if(pageNumber == 0) { pageNumber = 1; }

            return _dependencyService.Get(pageSize, pageNumber).WrapEnumerable();
        }
    }
}