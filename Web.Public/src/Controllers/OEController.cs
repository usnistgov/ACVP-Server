using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using Web.Public.Exceptions;
using Web.Public.Helpers;
using Web.Public.JsonObjects;
using Web.Public.Models;
using Web.Public.Results;
using Web.Public.Services;
using Web.Public.Services.MessagePayloadValidators;

namespace Web.Public.Controllers
{
	[Route("acvp/v1/oes")]
	public class OEController : JwtAuthControllerBase
	{
		private readonly IOEService _oeService;
		private readonly IMessageService _messageService;
		private readonly IJsonReaderService _jsonReader;
		private readonly IJsonWriterService _jsonWriter;
		private readonly IMessagePayloadValidatorFactory _workflowItemValidatorFactory;
		
		private readonly List<(string Property, bool IsNumeric, List<string> Operators)> _legalPropertyDefinitions = new List<(string Property, bool IsNumeric, List<string> Operators)> { 
			("name", false, new List<string> { "eq", "start", "end", "contains" })
		};

		public OEController(
			IJwtService jwtService,
			IOEService oeService,
			IMessageService messageService,
			IJsonReaderService jsonReader,
			IJsonWriterService jsonWriter,
			IMessagePayloadValidatorFactory workflowItemValidatorFactory)
			: base (jwtService)
		{
			_oeService = oeService;
			_messageService = messageService;
			_jsonReader = jsonReader;
			_jsonWriter = jsonWriter;
			_workflowItemValidatorFactory = workflowItemValidatorFactory;
		}
		
		[HttpPost]
		public async Task<JsonHttpStatusResult> CreateOE()
		{
			// Convert and validate
			var apiAction = APIAction.CreateOE;
			var payload = await _jsonReader.GetMessagePayloadFromBodyJsonAsync<OECreatePayload>(Request.Body, apiAction);
			var validation = _workflowItemValidatorFactory.GetMessagePayloadValidator(apiAction).Validate(payload);
			if (!validation.IsSuccess)
			{
				throw new PayloadValidatorException(validation.Errors);
			}
			
			// Pass to message queue
			var requestID = await _messageService.InsertIntoQueueAsync(apiAction, GetCertSubjectFromJwt(), payload);
			
			// Build request object for response
			var requestObject = new RequestObject
			{
				RequestID = requestID,
				Status = RequestStatus.Initial
			};
			
			return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(requestObject));
		}

		[HttpPut("{id}")]
		public async Task<JsonHttpStatusResult> UpdateOE(long id)
		{
			// Convert and validate
			var apiAction = APIAction.UpdateOE;
			var payload = await _jsonReader.GetMessagePayloadFromBodyJsonAsync<OEUpdatePayload>(Request.Body, apiAction);
			payload.ID = id;
			var validation = _workflowItemValidatorFactory.GetMessagePayloadValidator(apiAction).Validate(payload);
			if (!validation.IsSuccess)
			{
				throw new PayloadValidatorException(validation.Errors);
			}

			// Pass to message queue
			var requestID = await _messageService.InsertIntoQueueAsync(apiAction, GetCertSubjectFromJwt(), payload);
			
			// Build request object for response
			var requestObject = new RequestObject
			{
				RequestID = requestID,
				Status = RequestStatus.Initial
			};
			
			return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(requestObject));
		}

		[HttpDelete("{id}")]
		public async Task<JsonHttpStatusResult> DeleteOE(long id)
		{
			var apiAction = APIAction.DeleteOE;
			var payload = new DeletePayload()
			{
				ID = id
			};
			
			var validation = _workflowItemValidatorFactory.GetMessagePayloadValidator(apiAction).Validate(payload);
			if (!validation.IsSuccess)
			{
				throw new PayloadValidatorException(validation.Errors);
			}
			
			// Pass to message queue
			var requestID = await _messageService.InsertIntoQueueAsync(apiAction, GetCertSubjectFromJwt(), payload);
			
			// Build request object for response
			var requestObject = new RequestObject
			{
				RequestID = requestID,
				Status = RequestStatus.Initial
			};
			
			return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(requestObject));
		}
		
		[HttpGet("{id}")]
		public JsonHttpStatusResult GetOE(long id)
		{
			var oe = _oeService.GetOE(id);
			if (oe == null)
			{
				return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(new ErrorObject()
				{
					Error = Request.HttpContext.Request.Path,
					Context = $"Unable to find OE with id {id}."
				}), HttpStatusCode.NotFound);
			}
			
			return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(oe));
		}

		[HttpGet]
		public JsonHttpStatusResult GetFilteredOEList()
		{
			//Try to read limit and offset, if passed in
			var limit = 0;
			var offset = 0;
			if (Request.Query.TryGetValue("limit", out var stringLimit))
			{
				int.TryParse(stringLimit.First(), out limit);
			}

			if (Request.Query.TryGetValue("offset", out var stringOffset))
			{
				int.TryParse(stringOffset.First(), out offset);
			}

			//If limit was not present, or a garbage value, make it a default
			if (limit <= 0) limit = 20;

			//Try to parse the filter from the querystring
			var filter = FilterHelpers.ParseFilter(HttpUtility.UrlDecode(Request.QueryString.Value), _legalPropertyDefinitions);

			if (filter.IsValid)
			{
				var pagingOptions = new PagingOptions
				{
					Limit = limit,
					Offset = offset
				};

				var data = _oeService.GetFilteredList(filter.OrClauses, pagingOptions);
				var pagedData =  new PagedResponse<OperatingEnvironment>(data.TotalCount, data.OEs, "/acvp/v1/oes", pagingOptions, filter.QuerystringWithoutPaging);
				
				return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(pagedData));
			}

			var error = new ErrorObject
			{
				Error = "Invalid filter applied"
			};
			
			return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(error), HttpStatusCode.RequestedRangeNotSatisfiable);
		}
	}
}