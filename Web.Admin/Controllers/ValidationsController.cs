using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using Microsoft.AspNetCore.Mvc;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.Enumerables;

namespace Web.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValidationsController : ControllerBase
    {
        private readonly IValidationService _validationService;

        public ValidationsController(IValidationService validationService)
        {
            _validationService = validationService;
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
	}
}