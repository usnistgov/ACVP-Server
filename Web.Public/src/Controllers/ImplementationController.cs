using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;
using Web.Public.Exceptions;
using Web.Public.JsonObjects;
using Web.Public.Models;
using Web.Public.Results;
using Web.Public.Services;

namespace Web.Public.Controllers
{
	[Route("acvp/v1/modules")]
	[Authorize]
	[TypeFilter(typeof(ExceptionFilter))]
	[ApiController]
	public class ImplementationController : ControllerBase
	{
		private readonly IImplementationService _implementationService;
		private readonly IMessageService _messageService;
		private readonly IJsonReaderService _jsonReader;
		private readonly IJsonWriterService _jsonWriter;
		
		private readonly List<(string Property, bool IsNumeric, List<string> Operators)> _legalPropertyDefinitions = new List<(string Property, bool IsNumeric, List<string> Operators)> { 
			("name", false, new List<string> { "eq", "start", "end", "contains" }),
			("version", false, new List<string> { "eq", "start", "end", "contains" }),
			("website", false, new List<string> { "eq", "start", "end", "contains" }),
			("description", false, new List<string> { "eq", "start", "end", "contains" }),
			("type", false, new List<string> {"eq", "ne"}),
			("vendorId", true, new List<string> { "eq", "ne", "lt", "le", "gt", "ge" })
		};

		public ImplementationController(
			IImplementationService implementationService,
			IMessageService messageService,
			IJsonReaderService jsonReader,
			IJsonWriterService jsonWriter)
		{
			_implementationService = implementationService;
			_messageService = messageService;
			_jsonReader = jsonReader;
			_jsonWriter = jsonWriter;
		}

		[HttpPost]
		public JsonHttpStatusResult CreateImplementation()
		{
			// Get user cert
			var certRawData = Request.HttpContext.Connection.ClientCertificate.RawData;

			// Get raw JSON
			var jsonBlob = _jsonReader.GetJsonFromBody(Request.Body);
			
			// Convert to Dependency
			var implementation = _jsonReader.GetWorkflowItemPayloadFromBodyJson<ImplementationCreatePayload>(jsonBlob, APIAction.CreateImplementation);
			
			// Pass to message queue
			var requestID = _messageService.InsertIntoQueue(APIAction.CreateImplementation, certRawData, implementation);
			
			// Build request object for response
			var requestObject = new RequestObject
			{
				RequestID = requestID,
				Status = RequestStatus.Initial
			};
			
			return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(requestObject), HttpStatusCode.Accepted);
		}

		[HttpPut("{id}")]
		public JsonHttpStatusResult UpdateImplementation(long id)
		{
			// Get user cert
			var certRawData = Request.HttpContext.Connection.ClientCertificate.RawData;

			// Get raw JSON
			var jsonBlob = _jsonReader.GetJsonFromBody(Request.Body);
			
			// Convert to Dependency
			var implementation =
				_jsonReader.GetWorkflowItemPayloadFromBodyJson<ImplementationUpdatePayload>(
					jsonBlob, APIAction.UpdateImplementation, payload => payload.ID = id);
			
			// Pass to message queue
			var requestID = _messageService.InsertIntoQueue(APIAction.UpdateImplementation, certRawData, implementation);
			
			// Build request object for response
			var requestObject = new RequestObject
			{
				RequestID = requestID,
				Status = RequestStatus.Initial
			};
			
			return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(requestObject), HttpStatusCode.Accepted);
		}

		[HttpDelete("{id}")]
		public JsonHttpStatusResult DeleteImplementation(long id)
		{
			// Get user cert
			var certRawData = Request.HttpContext.Connection.ClientCertificate.RawData;

			// Get raw JSON
			var jsonBlob = _jsonReader.GetJsonFromBody(Request.Body);
			
			// Convert to Dependency
			var implementation = _jsonReader.GetWorkflowItemPayloadFromBodyJson<DeletePayload>(jsonBlob, APIAction.DeleteImplementation);

			implementation.ID = id;
			
			// Pass to message queue
			var requestID = _messageService.InsertIntoQueue(APIAction.DeleteImplementation, certRawData, implementation);
			
			// Build request object for response
			var requestObject = new RequestObject
			{
				RequestID = requestID,
				Status = RequestStatus.Initial
			};
			
			return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(requestObject), HttpStatusCode.Accepted);
		}
		
		[HttpGet("{id}")]
		public JsonHttpStatusResult GetImplementation(long id)
		{
			var result = _implementationService.GetImplementation(id);
			if (result == null)
			{
				return new JsonHttpStatusResult(
					_jsonWriter.BuildVersionedObject(
						new ErrorObject
						{
							Error = $"Unable to find implementation with id: {id}"
						}),
					HttpStatusCode.NotFound);
			}
			
			return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(result));
		}
		
		[HttpGet]
		public JsonHttpStatusResult GetFilteredImplementationList()
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

			//Build the querystring that excluded limit and offset
			var filterString = string.Join("&", Request.Query.Where(x => x.Key != "limit" && x.Key != "offset").Select(x => $"{x.Key}={x.Value.FirstOrDefault()}"));

			//Try to build a filter string from those parameters
			var filter = FilterStringService.BuildFilterString(filterString, _legalPropertyDefinitions);

			if (filter.IsValid)
			{
				var pagingOptions = new PagingOptions
				{
					Limit = limit,
					Offset = offset
				};

				var data = _implementationService.GetFilteredList(filter.FilterString, pagingOptions, filter.OrDelimiter, filter.AndDelimiter);
				var pagedData =  new PagedResponse<Implementation>(data.TotalCount, data.Implementations, "/acvp/v1/modules", pagingOptions, filterString);
				
				return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(pagedData));
			}

			var error = new ErrorObject
			{
				Error = $"Invalid filter applied: {filterString}"
			};
			
			return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(error), HttpStatusCode.RequestedRangeNotSatisfiable);
		}
	}
}