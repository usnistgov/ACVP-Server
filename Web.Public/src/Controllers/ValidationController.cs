using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Public.Exceptions;
using Web.Public.Results;
using Web.Public.Services;

namespace Web.Public.Controllers
{
	[Route("acvp/v1/validations")]
	[Authorize]
	[TypeFilter(typeof(ExceptionFilter))]
	[ApiController]
	public class ValidationController : ControllerBase
	{
		private readonly IValidationService _validationService;
		private readonly IJsonWriterService _jsonWriterService;

		public ValidationController(IValidationService validationService, IJsonWriterService jsonWriterService)
		{
			_validationService = validationService;
			_jsonWriterService = jsonWriterService;
		}
		
		[HttpGet("{id}")]
		public ActionResult GetValidation(long id)
		{
			var validation = _validationService.GetValidation(id);

			if (validation == null)
			{
				return new NotFoundResult();
			}

			return new JsonHttpStatusResult(_jsonWriterService.BuildVersionedObject(validation));
		}
	}
}