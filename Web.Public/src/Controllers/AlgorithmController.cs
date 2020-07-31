using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Public.Exceptions;
using Web.Public.JsonObjects;
using Web.Public.Results;
using Web.Public.Services;

namespace Web.Public.Controllers
{
	[Route("acvp/v1/algorithms")]
	[TypeFilter(typeof(ExceptionFilter))]
	[Authorize(AuthenticationSchemes = CertificateAuthenticationDefaults.AuthenticationScheme)]
	[ApiController]
	public class AlgorithmController : ControllerBase
	{
		private readonly IAlgorithmService _algoService;
		private readonly IJsonWriterService _jsonWriter;

		public AlgorithmController(IAlgorithmService algoService, IJsonWriterService jsonWriter)
		{
			_algoService = algoService;
			_jsonWriter = jsonWriter;
		}

		[HttpGet]
		public JsonHttpStatusResult GetAlgorithmList()
		{
			// Retrieve and return listing
			var list = _algoService.GetAlgorithmList();
			return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(new AlgorithmListObject
			{
				AlgorithmList = list.Select(x => new AlgorithmObject
				{
					AlgorithmId = x.AlgorithmId,
					Name = x.Name,
					Mode = x.Mode,
					Revision = x.Revision
				})
			}));
		}

		[HttpGet("{id}")]
		public JsonHttpStatusResult GetAlgorithmProperties(int id)
		{
			// TODO more information
			var algo = _algoService.GetAlgorithm(id);
			if (algo == null)
			{
				return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(new ErrorObject()
				{
					Error = Request.HttpContext.Request.Path,
					Context = $"Unable to find algorithm id: {id}."
				}), HttpStatusCode.NotFound);
			}

			if (algo == null)
			{
				return new JsonHttpStatusResult(
					_jsonWriter.BuildVersionedObject(
					new ErrorObject
					{
						Error = $"No algorithm matches provided id: {id}"
					}),
					HttpStatusCode.NotFound);
			}

			return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(new AlgorithmObject
			{
				AlgorithmId = algo.AlgorithmId,
				Name = algo.Name,
				Mode = algo.Mode,
				Revision = algo.Revision
			}));
		}
	}
}