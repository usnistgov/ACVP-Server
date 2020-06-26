using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using Microsoft.AspNetCore.Mvc;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.ExtensionMethods;
using NIST.CVP.Libraries.Internal.ACVPCore.Models;

namespace Web.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValidationsController : ControllerBase
    {
        private readonly IValidationService _validationService;
        private readonly IOEService _oeService;
        private readonly ICapabilityDisplayService _capabilityDisplayService;

        public ValidationsController(IValidationService validationService, IOEService oeService, ICapabilityDisplayService capabilityDisplayService)
        {
            _validationService = validationService;
            _oeService = oeService;
            _capabilityDisplayService = capabilityDisplayService;
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

        [HttpGet("{validationID}/validationOEAlgorithms")]
        public ActionResult<WrappedEnumerable<ValidationOEAlgorithmDisplay>> GetValidationOEAlgorithms(long validationID)
		{
            return _validationService.GetActiveValidationOEAlgorithmsForDisplay(validationID).ToWrappedEnumerable();

        }

        [HttpGet("{validationID}/validationOEAlgorithm/{validationOEAlgorithmID}")]
        public ActionResult<CapabilitiesDisplay> GetCapabilitiesDisplay(long validationOEAlgorithmID)
		{
            return _capabilityDisplayService.GetCapabilitiesDisplay(validationOEAlgorithmID);
		}
	}
}