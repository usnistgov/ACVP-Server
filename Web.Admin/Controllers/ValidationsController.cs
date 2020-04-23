using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using Microsoft.AspNetCore.Mvc;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.ExtensionMethods;

namespace Web.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValidationsController : ControllerBase
    {
        private readonly IValidationService _validationService;
        private readonly IOEService _oeService;

        public ValidationsController(IValidationService validationService, IOEService oeService)
        {
            _validationService = validationService;
            _oeService = oeService;
        }
        
        [HttpPost]
        public ActionResult<PagedEnumerable<ValidationLite>> GetValidations(ValidationListParameters param)
        {
            if (param == null)
                return new BadRequestResult();

            return _validationService.GetValidations(param);
        }

		[HttpGet("{validationId}")]
		public ActionResult<Validation> GetValidation(long validationId)
		{
			var result = _validationService.GetValidation(validationId);

			if (result == null)
				return new NotFoundResult();

			return result;
		}

        [HttpGet("{validationID}/oes")]
        public ActionResult<WrappedEnumerable<OperatingEnvironmentLite>> GetOEsOnValidation(long validationID)
        {
            return _oeService.GetOEsOnValidation(validationID).ToWrappedEnumerable();
        }
	}
}