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
	[Route("acvp/v1/dependencies")]
	[Authorize]
	[TypeFilter(typeof(ExceptionFilter))]
	[ApiController]
	public class DependencyController : ControllerBase
	{
		private readonly IDependencyService _dependencyService;
		private readonly IMessageService _messageService;
		private readonly IJsonReaderService _jsonReader;
		private readonly IJsonWriterService _jsonWriter;
		
		private readonly List<(string Property, bool IsNumeric, List<string> Operators)> _legalPropertyDefinitions = new List<(string Property, bool IsNumeric, List<string> Operators)> { 
			("name", false, new List<string> { "eq", "start", "end", "contains" }),
			("type", false, new List<string> { "eq", "start", "end", "contains" }),
			("description", false, new List<string> { "eq", "start", "end", "contains" })
		};

		public DependencyController(
			IDependencyService dependencyService,
			IMessageService messageService,
			IJsonReaderService jsonReader,
			IJsonWriterService jsonWriter)
		{
			_dependencyService = dependencyService;
			_messageService = messageService;
			_jsonReader = jsonReader;
			_jsonWriter = jsonWriter;
		}

		[HttpPost]
		public JsonHttpStatusResult CreateDependency()
		{
			// Get user cert
			var certRawData = Request.HttpContext.Connection.ClientCertificate.RawData;

			// Get raw JSON
			var jsonBlob = _jsonReader.GetJsonFromBody(Request.Body);
			
			// Convert to Dependency
			var dependency = _jsonReader.GetWorkflowItemPayloadFromBodyJson<DependencyCreatePayload>(jsonBlob, APIAction.CreateDependency);
			
			// Pass to message queue
			var requestID = _messageService.InsertIntoQueue(APIAction.CreateDependency, certRawData, dependency);
			
			// Build request object for response
			var requestObject = new RequestObject
			{
				RequestID = requestID,
				Status = RequestStatus.Initial
			};
			
			return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(requestObject), HttpStatusCode.Accepted);
		}

		[HttpPut("{id}")]
		public JsonHttpStatusResult UpdateDependency(long id)
		{
			// Get user cert
			var certRawData = Request.HttpContext.Connection.ClientCertificate.RawData;

			// Get raw JSON
			var jsonBlob = _jsonReader.GetJsonFromBody(Request.Body);
			
			// Convert to Dependency
			var dependency = 
				_jsonReader.GetWorkflowItemPayloadFromBodyJson<DependencyUpdatePayload>(
					jsonBlob, APIAction.UpdateDependency, payload => payload.ID = id );
			
			// Pass to message queue
			var requestID = _messageService.InsertIntoQueue(APIAction.UpdateDependency, certRawData, dependency);
			
			// Build request object for response
			var requestObject = new RequestObject
			{
				RequestID = requestID,
				Status = RequestStatus.Initial
			};
			
			return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(requestObject), HttpStatusCode.Accepted);
		}

		[HttpDelete("{id}")]
		public JsonHttpStatusResult DeleteDependency(long id)
		{
			// Get user cert
			var certRawData = Request.HttpContext.Connection.ClientCertificate.RawData;

			// Get raw JSON
			var jsonBlob = _jsonReader.GetJsonFromBody(Request.Body);
			
			// Convert to Dependency
			var dependency = _jsonReader.GetWorkflowItemPayloadFromBodyJson<DeletePayload>(jsonBlob, APIAction.DeleteDependency);

			dependency.ID = id;
			
			// Pass to message queue
			var requestID = _messageService.InsertIntoQueue(APIAction.DeleteDependency, certRawData, dependency);
			
			// Build request object for response
			var requestObject = new RequestObject
			{
				RequestID = requestID,
				Status = RequestStatus.Initial
			};
			
			return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(requestObject), HttpStatusCode.Accepted);
		}
		
		[HttpGet("{id}")]
		public JsonHttpStatusResult GetDependency(long id)
		{
			var dependency = _dependencyService.GetDependency(id);
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

				var data = _dependencyService.GetFilteredList(filter.FilterString, pagingOptions, filter.OrDelimiter, filter.AndDelimiter);
				var pagedData =  new PagedResponse<Dependency>(data.TotalCount, data.Dependencys, "/acvp/v1/dependencies", pagingOptions, filterString);
				
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