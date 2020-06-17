using System;
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
	[Route("acvp/v1/dependencies")]
	public class DependencyController : JwtAuthControllerBase
	{
		private readonly IDependencyService _dependencyService;
		private readonly IMessageService _messageService;
		private readonly IJsonReaderService _jsonReader;
		private readonly IJsonWriterService _jsonWriter;
		private readonly IMessagePayloadValidatorFactory _workflowItemValidatorFactory;
		
		private readonly List<(string Property, bool IsNumeric, List<string> Operators)> _legalPropertyDefinitions = new List<(string Property, bool IsNumeric, List<string> Operators)> { 
			("name", false, new List<string> { "eq", "start", "end", "contains" }),
			("type", false, new List<string> { "eq", "start", "end", "contains" }),
			("description", false, new List<string> { "eq", "start", "end", "contains" })
		};

		public DependencyController(
			IJwtService jwtService,
			IDependencyService dependencyService,
			IMessageService messageService,
			IJsonReaderService jsonReader,
			IJsonWriterService jsonWriter,
			IMessagePayloadValidatorFactory workflowItemValidatorFactory) 
			: base(jwtService)
		{
			_dependencyService = dependencyService;
			_messageService = messageService;
			_jsonReader = jsonReader;
			_jsonWriter = jsonWriter;
			_workflowItemValidatorFactory = workflowItemValidatorFactory;
		}

		[HttpPost]
		public async Task<JsonHttpStatusResult> CreateDependency()
		{
			// Convert and validate
			var apiAction = APIAction.CreateDependency;
			var payload = await _jsonReader.GetMessagePayloadFromBodyJsonAsync<DependencyCreatePayload>(Request.Body, apiAction);
			var validation = _workflowItemValidatorFactory.GetMessagePayloadValidator(apiAction).Validate(payload);
			if (!validation.IsSuccess)
			{
				throw new JsonReaderException(validation.Errors);
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
		public async Task<JsonHttpStatusResult> UpdateDependency(long id)
		{
			// Convert and validate
			var apiAction = APIAction.UpdateDependency;
			var payload = await _jsonReader.GetMessagePayloadFromBodyJsonAsync<DependencyUpdatePayload>(Request.Body, apiAction);
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
		public async Task<JsonHttpStatusResult> DeleteDependency(long id)
		{
			var apiAction = APIAction.DeleteDependency;
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
		public JsonHttpStatusResult GetDependency(long id)
		{
			var dependency = _dependencyService.GetDependency(id);
			if (dependency == null)
			{
				return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(new ErrorObject()
				{
					Error = Request.HttpContext.Request.Path,
					Context = $"Unable to find dependency with id {id}."
				}), HttpStatusCode.NotFound);
			}
			
			return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(dependency));
		}

		[HttpGet]
		public JsonHttpStatusResult GetFilteredDependencyList()
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

				var data = _dependencyService.GetFilteredList(filter.OrClauses, pagingOptions);
				var pagedData =  new PagedResponse<Dependency>(data.TotalCount, data.Dependencys, "/acvp/v1/dependencies", pagingOptions, filter.QuerystringWithoutPaging);
				
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