using ACVPCore.ExtensionMethods;
using ACVPCore.Models;
using ACVPCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Admin.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ValidationController : ControllerBase
	{
		private readonly IValidationService _validationService;

		public ValidationController(IValidationService validationService)
		{
			_validationService = validationService;
		}

		[HttpGet]
		public WrappedEnumerable<ValidationLite> GetValidations()
		{
			return _validationService.GetValidations().WrapEnumerable();
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