using ACVPCore.ExtensionMethods;
using ACVPCore.Services;
using Microsoft.AspNetCore.Mvc;
using ACVPCore.Models;
using System;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperatingEnvironmentsController : ControllerBase
    {
        private readonly IOEService _oeService;

        public OperatingEnvironmentsController(IOEService oeService)
        {
            _oeService = oeService;
        }

        [HttpGet("{oeID}")]
        public OperatingEnvironment Get(long oeID)
        {
            return _oeService.Get(oeID);
        }

        [HttpPost("{oeID}/Dependencies")]
        public Result AddDependencyLink(long oeID, [FromBody] OEDependencyLinkCreateParameters parameters)
        {
            return _oeService.AddDependencyLink(oeID, parameters.DependencyID);
        }

        [HttpDelete("{oeID}/Dependencies/{dependencyID}")]
        public Result RemoveDependencyLink(long oeID, long dependencyID)
        {
            return _oeService.RemoveDependencyLink(oeID, dependencyID);
        }

        [HttpPost]
        public ActionResult<PagedEnumerable<OperatingEnvironmentLite>> GetOEs(OeListParameters param)
        {
            if (param == null)
                return new BadRequestResult();

            return _oeService.Get(param);
        }

        [HttpPatch("{oeID}")]
        public Result UpdateOE(OperatingEnvironment oe, long oeID)
        {
            var updateParams = new OEUpdateParameters
            {
                ID = oeID
            };

            if (oe.Name != null)
            {
                updateParams.Name = oe.Name;
                updateParams.NameUpdated = true;
            }

            return _oeService.Update(updateParams);
        }
    }
}
