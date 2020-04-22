using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using Microsoft.AspNetCore.Mvc;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.Results;

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
